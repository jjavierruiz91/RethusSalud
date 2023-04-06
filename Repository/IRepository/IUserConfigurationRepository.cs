using rethus_backend.Models;
using rethus_backend.Models.Dto.Configuration;

namespace rethus_backend.Repository.IRepository
{
  public interface IUserConfigurationRepository : IRepository<Configurations>
  {
    bool IsUniqueUser(string userId);
    // Task<UserResponseDto> Login(UserRequestDto loginRequestDTO);
    Task<Configurations> Register(ConfigurationCreateDto createRequestDto);
  }
}