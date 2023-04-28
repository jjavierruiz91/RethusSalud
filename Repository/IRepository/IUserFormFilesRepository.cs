using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;

namespace rethus_backend.Repository.IRepository
{
  public interface IUserFormFilesRepository : IRepository<UserFormFiles>
  {
    bool IsUnique(string userFormId);

    bool IsExist(string userFormId);

    UserFormFiles GetById(string id);

    IEnumerable<UserFormFiles> GetAll();
    ApiResponse RegisterUserFormFile(string userFormId, UserFormFilesCreateDto payload);
  }
}