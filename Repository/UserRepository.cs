using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(ApplicationDbContext db) : base(db)
    {
    }

    public bool IsUniqueUser(string username)
    {
      throw new NotImplementedException();
    }

    public Task<UserResponseDto> Login(UserRequestDto loginRequestDTO)
    {
      throw new NotImplementedException();
    }

    public Task<User> Register(CreateRequestDto createRequestDto)
    {
      throw new NotImplementedException();
    }
  }
}