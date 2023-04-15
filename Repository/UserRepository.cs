using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Security.Hashing;
namespace rethus_backend.Repository
{
  public class UserRepository : Repository<User>, IUserRepository
  {

    private readonly ApplicationDbContext _context;
    private readonly IUserConfigurationRepository _configuration;

    public UserRepository(ApplicationDbContext db, IUserConfigurationRepository configuration) : base(db)
    {
      _context = db;
      _configuration = configuration;
    }

    public IEnumerable<User> GetAll()
    {
      return _context.Users;
    }

    public User GetById(string email)
    {
      return _context.Users.FirstOrDefault(user => user.email == email);
    }

    public User GetUserByEmail(string id)
    {
      return _context.Users.Find(id);
    }

    public bool IsExistUser(string email)
    {
      User user = _context.Users.FirstOrDefault(x => x.email == email);
      if (user == null) return false;
      return true;
    }

    public bool IsUniqueUser(string email)
    {
      User user = _context.Users.FirstOrDefault(x => x.email == email);
      return user == null;
    }

    public async Task<User> Register(CreateRequestDto createRequestDto)
    {
      byte[] passwordHash, passwordSalt;
      HashingHelper.CreatePasswordHash(createRequestDto.password, out passwordHash, out passwordSalt);

      User newUser = new()
      {
        name = createRequestDto.name,
        email = createRequestDto.email,
        PasswordHash = passwordHash,
        PasswordSalt = passwordSalt,
        Roles = "user",
        status = "active",
        Token = "",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      var createRegister = _context.Users.Add(newUser);
      _context.SaveChanges();

      this._configuration.Register(createRequestDto.email);

      return newUser;
    }
  }
}