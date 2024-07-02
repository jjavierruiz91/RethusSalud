using rethus_backend.Repository.IRepository.Auth;
using rethus_backend.Models;
using rethus_backend.Data;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using rethus_backend.Models.Dto.Auth;
using rethus_backend.Utilities.Security.Hashing;
using Microsoft.IdentityModel.Tokens;

namespace rethus_backend.Repository
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string secretKey;

        public AuthRepository(ApplicationDbContext db, IConfiguration configuration)
            : base(db)
        {
            _context = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<AuthResponseDto> Authenticate(AuthRequestDto _user)
        {
            var user = _context.Users.SingleOrDefault(x => x.email == _user.email);

            if (user == null)
            {
                return null;
            }

            if (
                !HashingHelper.VerifyPasswordHash(
                    _user.password,
                    user.PasswordHash,
                    user.PasswordSalt
                )
            )
            {
                return null;
            }
            var user_configuration = _context.Configurations.SingleOrDefault(
                x => x.UserId == user.UserId
            );

            if (user_configuration == null)
            {
                return null;
            }

            var jwtToken = generateJwtToken(user);
            _context.Update(user);
            _context.SaveChanges();

            return new AuthResponseDto(user, jwtToken, user_configuration);
        }

        public string generateJwtToken(User _user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.name),
                new Claim("role", _user.roles),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            System.Diagnostics.Debug.WriteLine(securityKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var key = Encoding.ASCII.GetBytes(secretKey);
            var SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );

            //  Expires = DateTime.UtcNow.AddHours(15),
            // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            // Subject = this.GenerateClaims(_user),
            // var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
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

        private ClaimsIdentity GenerateClaims(User user)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("email", user.email));
            ci.AddClaim(new Claim("id", user.UserId));

            ci.AddClaim(new Claim("role", user.roles));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            return ci;
        }

        public string generateJwtTokenTemp(string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            System.Diagnostics.Debug.WriteLine(securityKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var key = Encoding.ASCII.GetBytes(secretKey);
            var SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );

            //  Expires = DateTime.UtcNow.AddHours(15),
            // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            // Subject = this.GenerateClaims(_user),
            // var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool validateJwtTokenTmep(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Cambia a true y configura ValidIssuer si estás validando el emisor
                ValidateAudience = false, // Cambia a true y configura ValidAudience si estás validando el destinatario
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero // Para evitar problemas de desfase de tiempo
            };

            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out SecurityToken validatedToken
                );
                return true;
            }
            catch (Exception ex)
            {
                // El token no es válido
                Console.WriteLine($"Token inválido: {ex.Message}");
                return false;
            }
        }
    }
}
