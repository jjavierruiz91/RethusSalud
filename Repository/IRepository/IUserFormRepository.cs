using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;

namespace rethus_backend.Repository.IRepository
{
  public interface IUserFormRepository : IRepository<UserForm>
  {
    bool IsUniqueUser(string userFormId);

    bool IsExistUser(string userFormId);

    UserForm GetById(string id);

    PaginationResult<UserForm> GetAll(int? page);
    Task<ApiResponse> post(UserFormCreateDto createRequestDto);

    ApiResponse RegisterUserFormFile(string userFormId, UserFormFilesCreateDto payload);
  }
}