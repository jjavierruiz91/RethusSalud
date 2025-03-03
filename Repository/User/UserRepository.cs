using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.UserPublic;
using rethus_backend.Repository.IRepository;
using rethus_backend.RepositoryV2;
using rethus_backend.Utilities.Constants.Email.EmailDto;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.Email;
using rethus_backend.Utilities.Email.EmailService;
using rethus_backend.Utilities.FileHelper;
using rethus_backend.Utilities.Security.Hashing;

namespace rethus_backend.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _config;
        private readonly IUserConfigurationRepository _configuration;
        private readonly PaginationService<
            User,
            UserQueryParametersDto,
            UserDto
        > _paginationService;

        private readonly IPaginationRepositoryV2<User> _repositoryPaginationV2;

        public UserRepository(
            ApplicationDbContext db,
            IConfiguration config,
            IUserConfigurationRepository configuration,
            IPaginationRepositoryV2<User> repositoryPaginationV2
        )
            : base(db)
        {
            _context = db;
            _config = config;
            _configuration = configuration;
            _repositoryPaginationV2 = repositoryPaginationV2;
        }

        public PaginationResultDto<UserDto> GetPagination(
            PaginationRequestDto<UserQueryParametersDto> request,
            Func<User, UserDto> mapper
        )
        {
            var result = _paginationService.GetPaginatedEntities(request, mapper);

            return result;
        }

        public User GetById(string userId)
        {
            return _context.Users.FirstOrDefault(user => user.UserId == userId);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.email == email);
        }

        public bool isExistUserCount(string email)
        {
            bool userExist = _context.Users.Any(x => x.email == email);
            return userExist;
        }

        public bool IsExistUser(string email)
        {
            User user = _context.Users.FirstOrDefault(x => x.email == email);
            if (user == null)
                return false;
            return true;
        }

        public bool IsExistUserId(string userId)
        {
            bool userExist = _context.Users.Any(x => x.UserId == userId);

            return userExist;
        }

        public bool IsValidRole(UserRoles rol)
        {
            return Enum.IsDefined(typeof(UserRoles), rol);
        }

        public bool IsUniqueUser(string email)
        {
            User user = _context.Users.FirstOrDefault(x => x.email == email);
            return user == null;
        }

        public async Task<User> Register(CreateRequestDto createRequestDto)
        {
            byte[] passwordHash,
                passwordSalt;
            HashingHelper.CreatePasswordHash(
                createRequestDto.password,
                out passwordHash,
                out passwordSalt
            );

            User newUser =
                new()
                {
                    name = createRequestDto.name,
                    email = createRequestDto.email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    roles = "User",
                    Status = UserStatus.active,
                    Token = "",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

            var createRegister = _context.Users.Add(newUser);
            _context.SaveChanges();

            this._configuration.Register(createRequestDto.email);

            return newUser;
        }

        public bool ValidateUserRole(string userType)
        {
            if (Enum.TryParse<UserRoles>(userType, out var userRole))
            {
                return true;
            }

            return false;
        }

        public bool ValidateUserStatus(string userStatus)
        {
            if (Enum.TryParse<UserStatus>(userStatus, out var user))
            {
                return true;
            }

            return false;
        }

        public async Task<User> RegisterUserAdministration(
            CreateUserAdministrativeRequestDto createRequestDto
        )
        {
            byte[] passwordHash,
                passwordSalt;
            HashingHelper.CreatePasswordHash(
                createRequestDto.password,
                out passwordHash,
                out passwordSalt
            );

            User newUser =
                new()
                {
                    name = createRequestDto.name,
                    email = createRequestDto.email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    roles = createRequestDto.type,
                    Status = UserStatus.active,
                    Token = "",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

            var createRegister = _context.Users.Add(newUser);
            _context.SaveChanges();

            this._configuration.Register(createRequestDto.email);

            return newUser;
        }

        public async Task<User> UpdateUserAdministration(
            UpdateUserAdministrativeRequestDto updateRequestDto,
            User _user
        )
        {
            _user.name = updateRequestDto.name;
            _user.roles = updateRequestDto.type;
            _user.UpdatedAt = DateTime.Now;
            _user.Status = UserConstants.getFormatStringToUserStatus(updateRequestDto.status);

            if (updateRequestDto.password != null)
            {
                byte[] passwordHash,
                    passwordSalt;
                HashingHelper.CreatePasswordHash(
                    updateRequestDto.password,
                    out passwordHash,
                    out passwordSalt
                );

                _user.PasswordHash = passwordHash;
                _user.PasswordSalt = passwordSalt;
            }
            var updatedUser = _context.Users.Update(_user);
            _context.SaveChanges();

            return updatedUser.Entity;
        }

        public async Task<User> UpdateStatusUserAdministaration(User _user, UserStatus newStatus)
        {
            _user.Status = newStatus;

            var updatedUser = _context.Users.Update(_user);
            _context.SaveChanges();

            return updatedUser.Entity;
        }

        public User GetByUserId(string UserId)
        {
            return _context.Users.First(user => user.UserId == UserId);
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsUserActive(string email)
        {
            var userActive = _context.Users.Count(
                user => user.email == email && user.Status == UserStatus.active
            );

            return userActive > 0;
        }

        public async Task<bool> restorePassword(RestoreSendEmailUser payload)
        {
            try
            {
                var filePath = _config.GetSection("routeTemplateRestorePassword").Value;

                SendEmailDto payloadSendEmail = new SendEmailDto
                {
                    IsBodyHtml = true,
                    Subject = "Restablecer contrasena",
                    To = new List<string> { payload.Email },
                };
                var EmailServer = new EmailService(_config);

                TemplateConfigurationDto ConfigTemplate = new TemplateConfigurationDto
                {
                    TemplatePashEmail = filePath,
                    Token = payload.Token
                };

                var config = await EmailServer.ConfigurationTemplateRestorePassword(ConfigTemplate);
                payloadSendEmail.TemplateEmail = config;

                await EmailServer.SendEmail(payloadSendEmail);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public User GetUserByToken(string token)
        {
            return _context.Users.First(user => user.Token == token);
        }

        public async Task<bool> updatePassword(UserPayloadPassword payload)
        {
            var user = _context.Users.Single(x => x.UserId == payload.UserId);

            if (user == null)
            {
                return false;
            }

            byte[] passwordHash,
                passwordSalt;
            HashingHelper.CreatePasswordHash(
                payload.newPassword,
                out passwordHash,
                out passwordSalt
            );

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();
            return true;
        }

        public bool UpdateTokenUser(string userId, string token)
        {
            var user = _context.Users.Single(x => x.UserId == userId);

            user.Token = token;
            var updatedUser = _context.Users.Update(user);

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<PaginationResultDto<TResult>> GetPagedData<TResult>(
            PaginationRequestDto<FilterQueryParametersDto> request,
            Func<User, TResult> selector
        )
        {
            return await _repositoryPaginationV2.GetPagedAsync(request, selector);
        }
    }
}
