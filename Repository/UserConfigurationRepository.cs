using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Configuration;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UserConfigurationRepository : Repository<Configurations>, IUserConfigurationRepository
  {
    public UserConfigurationRepository(ApplicationDbContext db) : base(db)
    {
    }

    public bool IsUniqueUser(string userId)
    {
      throw new NotImplementedException();
    }

    public Task<Configurations> Register(ConfigurationCreateDto createRequestDto)
    {
      throw new NotImplementedException();
    }
  }
}