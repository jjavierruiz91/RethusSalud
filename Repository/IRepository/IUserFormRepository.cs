using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;

namespace rethus_backend.Repository.IRepository
{
  public interface IUserFormRepository : IRepository<UserForm>
  {
    bool IsUniqueUser(string userId);
    // Task<UserResponseDto> Login(UserRequestDto loginRequestDTO);
    Task<UserForm> Register(UserFormCreateDto createRequestDto);
  }
}