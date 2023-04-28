using rethus_backend.Data;
using rethus_backend.Repository.IRepository;
using rethus_backend.Repository.IRepository.Auth;

namespace rethus_backend.Repository
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db, IConfiguration _configuration)
    {
      _db = db;
      UserConfiguration = new UserConfigurationRepository(_db);
      UserConfiguration = new UserConfigurationRepository(_db);
      User = new UserRepository(_db, UserConfiguration);
      Auth = new AuthRepository(_db, _configuration);
      UserForm = new UserFormRepository(_db, _configuration);
      UserFormFiles = new UserFormFilesRepository(_db, _configuration, UserForm);
    }
    public IUserRepository User { get; private set; }

    public IUserConfigurationRepository UserConfiguration { get; private set; }

    public IUserFormRepository UserForm { get; private set; }

    public IUserFormFilesRepository UserFormFiles { get; private set; }

    public IAuthRepository Auth { get; private set; }

    public void Dispose() => _db.Dispose();

    public void Save()
    {
      _db.SaveChanges();
    }
  }
}