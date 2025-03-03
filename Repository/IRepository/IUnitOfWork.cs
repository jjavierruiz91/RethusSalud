using rethus_backend.Repository.IRepository.Auth;

namespace rethus_backend.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IUserConfigurationRepository UserConfiguration { get; }
        IUserFormRepository UserForm { get; }
        IUserFormFilesRepository UserFormFiles { get; }
        IAuthRepository Auth { get; }
        ICommentsRepository Comments { get; }
        ICountryRepository Country { get; }
        IDepartmentRepository Department { get; }
        ICityRepository City { get; }
        IConfigurationSettingRepository ConfigurationSetting { get; }
        IUserDigitalSignatureRepository UserDigitalSignature { get; }
        void Save();
    }
}
