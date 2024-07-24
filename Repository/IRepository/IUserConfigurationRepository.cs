using rethus_backend.Models;
using rethus_backend.Models.Dto.Configuration;

namespace rethus_backend.Repository.IRepository
{
    public interface IUserConfigurationRepository : IRepository<Configurations>
    {
        bool IsUniqueUser(string userId);

        // Task<UserResponseDto> Login(UserRequestDto loginRequestDTO);
        // Task<Configurations> Register(ConfigurationCreateDto createRequestDto);
        Configurations GetById(string id);
        Configurations GetByUserId(string userId);

        ApiResponse updateStepConfiguration(string id, ConfigurationStep step);
        ApiResponse updateTypeProcessConfiguration(
            string id,
            ConfigurationTypeProcedure type_procedure
        );
        ApiResponse updateTermConditionsConfiguration(string id, bool term);
        ApiResponse updateAutomaticStepConfiguration(string id);
        ApiResponse updateAutomaticStateConfiguration(string id);
        void Register(string email);
    }
}
