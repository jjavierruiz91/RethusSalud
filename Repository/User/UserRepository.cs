using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.Security.Hashing;

namespace rethus_backend.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserConfigurationRepository _configuration;
        private readonly PaginationService<
            User,
            UserQueryParametersDto,
            UserDto
        > _paginationService;
        private IUserConfigurationRepository userConfiguration;

        public UserRepository(
            ApplicationDbContext db,
            IUserConfigurationRepository configuration,
            PaginationService<User, UserQueryParametersDto, UserDto> paginationService
        )
            : base(db)
        {
            _context = db;
            _configuration = configuration;
            _paginationService = paginationService;
        }

        public PaginationResultDto<UserDto> GetPagination(
            PaginationRequestDto<UserQueryParametersDto> request,
            Func<User, UserDto> mapper
        )
        {
            var result = _paginationService.GetPaginatedEntities(request, mapper);

            return result;
        }

        public User GetById(string email)
        {
            return _context.Users.FirstOrDefault(user => user.email == email);
        }

        public User GetUserByEmail(string id)
        {
            return _context.Users.Find(id);
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
            Console.WriteLine(newStatus);
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
    }
}
