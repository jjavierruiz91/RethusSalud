using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;

namespace rethus_backend.Repository.IRepository
{
  public interface IUserFormRepository : IRepository<UserForm>
  {
    bool IsUniqueUser(string userFormId);

    bool IsExistUser(string userFormId);
    Task<ApiResponse> post(UserFormCreateDto createRequestDto);

    UserForm GetById(string id);

    IEnumerable<UserForm> GetAll();
  }
}