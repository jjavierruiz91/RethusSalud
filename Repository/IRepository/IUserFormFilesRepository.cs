using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Utilities.Templates.dto;

namespace rethus_backend.Repository.IRepository
{
    public interface IUserFormFilesRepository : IRepository<UserFormFiles>
    {
        bool IsUnique(string userFormId);

        bool IsExist(string userFormId);
        Task<List<byte[]>> GetFilesByUserFormId(string userFormId);
        List<GetUserFormIdDto> GetUserFormId(string userFormId);
        Task<UserFormFileDetails> GetFileByUserFormId(string userFormFileId);

        IEnumerable<UserFormFiles> GetAll();

        Task<ApiResponse> RegisterUserFormFileAsync(string userId, UserFormFilesCreateDto payload);
    }
}
