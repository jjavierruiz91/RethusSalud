using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UserRepository : Repository<User>, IUserRepository
  {

    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext db) : base(db)
    {
      _context = db;
    }

    public IEnumerable<User> GetAll()
    {
      return _context.Users;
    }

    public User GetById(string id)
    {
      return _context.Users.Find(id);
    }

    public bool IsUniqueUser(string email)
    {
      User user = _context.Users.FirstOrDefault(x => x.email == email);
      return user == null;
    }

    public async Task<User> Register(CreateRequestDto createRequestDto)
    {
      User newUser = new()
      {
        name = createRequestDto.name,
        email = createRequestDto.email,
        password = createRequestDto.password,
        Roles = "user",
        status = "active",
        Token = "",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      _context.Users.Add(newUser);
      _context.SaveChanges();

      return newUser;
    }
  }
}