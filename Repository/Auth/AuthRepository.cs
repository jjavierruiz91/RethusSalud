using rethus_backend.Repository.IRepository.Auth;
using rethus_backend.Models;
using rethus_backend.Data;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using rethus_backend.Models.Dto.Auth;

namespace rethus_backend.Repository
{
  public class AuthRepository : Repository<User>, IAuthRepository
  {
    private readonly ApplicationDbContext _context;
    private readonly string secretKey;

    public AuthRepository(ApplicationDbContext db, IConfiguration configuration) : base(db)
    {
      _context = db;
      secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }

    public async Task<AuthResponseDto> Authenticate(AuthRequestDto _user)
    {
      var user = _context.Users.SingleOrDefault(x => x.email == _user.email && x.password == _user.password);

      if (user == null)
      {
        return null;
      }

      var jwtToken = generateJwtToken(user);
      _context.Update(user);
      _context.SaveChanges();

      return new AuthResponseDto(user, jwtToken);
    }

    public string generateJwtToken(User _user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(secretKey);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Name, _user.email.ToString()),
                    new Claim(ClaimTypes.Sid, _user.UserId.ToString())
          }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public IEnumerable<User> GetAll()
    {
      return _context.Users;
    }

    public User GetById(int id)
    {
      return _context.Users.Find(id);
    }

    public AuthResponseDto RefreshToken(string token)
    {
      throw new NotImplementedException();
    }

    public bool RevokeToken(string token, string ipAddress)
    {
      throw new NotImplementedException();
    }


  }
}