using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UserFormRepository : Repository<UserForm>, IUserFormRepository
  {
    public UserFormRepository(ApplicationDbContext db) : base(db)
    {
    }

    public bool IsUniqueUser(string userId)
    {
      throw new NotImplementedException();
    }

    public Task<UserForm> Register(UserFormCreateDto createRequestDto)
    {
      throw new NotImplementedException();
    }
  }
}