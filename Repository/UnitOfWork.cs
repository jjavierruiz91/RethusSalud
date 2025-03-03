using Microsoft.Extensions.Caching.Memory;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Repository.IRepository;
using rethus_backend.Repository.IRepository.Auth;
using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        private readonly IMemoryCache _memoryCache;

        public UnitOfWork(
            ApplicationDbContext db,
            IConfiguration _configuration,
            IMemoryCache memoryCache
        )
        {
            _db = db;
            _memoryCache = memoryCache;

            UserConfiguration = new UserConfigurationRepository(_db);

            var paginationUserRepositoryV2 = new RepositoryPaginationV2<User>(_db);

            User = new UserRepository(
                _db,
                _configuration,
                UserConfiguration,
                paginationUserRepositoryV2
            );
            Auth = new AuthRepository(_db, _configuration);

            var paginationRepositoryV2 = new RepositoryPaginationV2<UserForm>(_db);

            UserDigitalSignature = new UserDigitalSignatureRepository(_db);

            UserForm = new UserFormRepository(
                _db,
                _configuration,
                UserConfiguration,
                paginationRepositoryV2,
                UserDigitalSignature
            );

            UserFormFiles = new UserFormFilesRepository(_db, _configuration, UserConfiguration);

            var paginationRepositoryComments = new RepositoryPaginationV2<Comments>(_db);

            Comments = new CommentsRepository(_db, User, UserForm, paginationRepositoryComments);

            Country = new CountryRepository(_db, _memoryCache);

            Department = new DepartmentRepository(_db, _memoryCache);

            City = new CityRepository(_db, _memoryCache);

            ConfigurationSetting = new ConfigurationSettingRepository(_db, _memoryCache);
        }

        public IUserRepository User { get; private set; }

        public IUserConfigurationRepository UserConfiguration { get; private set; }

        public IUserFormRepository UserForm { get; private set; }

        public IUserFormFilesRepository UserFormFiles { get; private set; }

        public IAuthRepository Auth { get; private set; }

        public ICommentsRepository Comments { get; private set; }

        public ICountryRepository Country { get; private set; }

        public IDepartmentRepository Department { get; private set; }
        public ICityRepository City { get; private set; }
        public IConfigurationSettingRepository ConfigurationSetting { get; private set; }
        public IUserDigitalSignatureRepository UserDigitalSignature { get; private set; }

        public void Dispose() => _db.Dispose();

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
