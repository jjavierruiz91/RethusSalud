using Microsoft.EntityFrameworkCore.ChangeTracking;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.UserPublic;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsUniqueUser(string email);
        bool IsExistUser(string email);
        bool IsExistUserId(string userId);
        bool isExistUserCount(string email);
        bool IsUserActive(string email);
        User GetUserByEmail(string email);
        User GetUserByToken(string token);
        IEnumerable<User> GetAll();
        public PaginationResultDto<UserDto> GetPagination(
            PaginationRequestDto<UserQueryParametersDto> request,
            Func<User, UserDto> mapper
        );
        User GetById(string id);
        User GetByUserId(string UserId);
        Task<User> Register(CreateRequestDto createRequestDto);
        Task<User> RegisterUserAdministration(CreateUserAdministrativeRequestDto createRequestDto);
        Task<User> UpdateUserAdministration(
            UpdateUserAdministrativeRequestDto updateRequestDto,
            User _user
        );
        Task<User> UpdateStatusUserAdministaration(User _user, UserStatus newStatus);
        bool ValidateUserRole(string userType);
        bool ValidateUserStatus(string userStatus);

        Task<bool> restorePassword(RestoreSendEmailUser payload);
        Task<Boolean> updatePassword(UserPayloadPassword payload);

        bool UpdateTokenUser(string userId, string token);

        Task<PaginationResultDto<TResult>> GetPagedData<TResult>(
            PaginationRequestDto<FilterQueryParametersDto> request,
            Func<User, TResult> selector
        );
    }
}
