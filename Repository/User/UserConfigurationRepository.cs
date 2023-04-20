using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Configuration;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UserConfigurationRepository : Repository<Configurations>, IUserConfigurationRepository
  {
    private readonly ApplicationDbContext _context;

    public UserConfigurationRepository(ApplicationDbContext db) : base(db)
    {
      _context = db;
    }

    public bool IsUniqueUser(string userId)
    {
      throw new NotImplementedException();
    }

    public void Register(string email)
    {
      var user = this._context.Users.FirstOrDefault(user => user.email == email);

      Configurations newConfiguration = new()
      {
        state = "en proceso",
        step = "etapa 1",
        UserId = user.UserId,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      _context.Configurations.Add(newConfiguration);
      _context.SaveChanges();
    }
  }
}