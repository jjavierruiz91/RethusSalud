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

        public UnitOfWork(ApplicationDbContext db, IConfiguration _configuration)
        {
            _db = db;
            UserConfiguration = new UserConfigurationRepository(_db);

            var paginationService = new PaginationService<User, UserQueryParametersDto, UserDto>(
                _db
            );
            User = new UserRepository(_db, _configuration, UserConfiguration, paginationService);
            Auth = new AuthRepository(_db, _configuration);

            var paginationUserFormService = new PaginationService<
                UserForm,
                CommonQueryParametersDto,
                UserFormResponseDto
            >(_db);

            UserForm = new UserFormRepository(_db, _configuration, paginationUserFormService);
            UserFormFiles = new UserFormFilesRepository(_db, _configuration, UserForm);

            var paginationCommentsService = new PaginationService<
                Comments,
                CommonQueryParametersDto,
                CommentsResponseDto
            >(_db);
            Comments = new CommentsRepository(_db, User, UserForm, paginationCommentsService);

            Country = new CountryRepository(_db);
        }

        public IUserRepository User { get; private set; }

        public IUserConfigurationRepository UserConfiguration { get; private set; }

        public IUserFormRepository UserForm { get; private set; }

        public IUserFormFilesRepository UserFormFiles { get; private set; }

        public IAuthRepository Auth { get; private set; }

        public ICommentsRepository Comments { get; private set; }

        public ICountryRepository Country { get; private set; }

        public IDepartmentRepository Department { get; private set; }

        public void Dispose() => _db.Dispose();

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
