using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UserFormFilesRepository : Repository<UserFormFiles>, IUserFormFilesRepository
  {
    public UserFormFilesRepository(ApplicationDbContext db) : base(db)
    {
    }

    public bool IsUniqueUser(string userId)
    {
      throw new NotImplementedException();
    }

    public Task<UserForm> Register(UserFormFilesCreateDto createRequestDto)
    {
      throw new NotImplementedException();
    }
  }
}