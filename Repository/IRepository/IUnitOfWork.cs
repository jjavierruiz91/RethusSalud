
namespace rethus_backend.Repository.IRepository
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository User { get; }
    IUserConfigurationRepository UserConfiguration { get; }
    IUserFormRepository UserForm { get; }
    IUserFormFilesRepository UserFormFiles { get; }

    void Save();
  }
}