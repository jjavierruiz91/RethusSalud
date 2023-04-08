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

    public bool IsUniqueUser(string email)
    {
      User user = _context.Users.FirstOrDefault(x => x.email == email);
      return user == null;
    }

    public Task<UserResponseDto> Login(UserRequestDto loginRequestDTO)
    {
      throw new NotImplementedException();
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