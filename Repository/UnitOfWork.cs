using rethus_backend.Data;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db, IConfiguration _configuration)
    {
      _db = db;
      User = new UserRepository(_db);
      UserConfiguration = new UserConfigurationRepository(_db);
      UserForm = new UserFormRepository(_db);
      UserFormFiles = new UserFormFilesRepository(_db);
    }
    public IUserRepository User { get; private set; }

    public IUserConfigurationRepository UserConfiguration { get; private set; }

    public IUserFormRepository UserForm { get; private set; }

    public IUserFormFilesRepository UserFormFiles { get; private set; }

    public void Dispose() => _db.Dispose();

    public void Save()
    {
      _db.SaveChanges();
    }
  }
}