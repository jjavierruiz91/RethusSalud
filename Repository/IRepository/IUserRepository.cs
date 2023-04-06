using rethus_backend.Models;
using rethus_backend.Models.Dto.User;

namespace rethus_backend.Repository.IRepository
{
  public interface IUserRepository : IRepository<User>
  {
    bool IsUniqueUser(string username);
    Task<UserResponseDto> Login(UserRequestDto loginRequestDTO);
    Task<User> Register(CreateRequestDto createRequestDto);
  }
}