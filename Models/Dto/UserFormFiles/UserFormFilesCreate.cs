using System.ComponentModel.DataAnnotations;
using rethus_backend.Utilities.Constants.User.UserFormConstants;

namespace rethus_backend.Models.Dto.UserFormFiles
{
    public class FileUpload
    {
        [Required(ErrorMessage = "id is required")]
        public TypeUploadFile Id { get; set; }

        [Required(ErrorMessage = "file is required")]
        public IFormFile File { get; set; }
    }

    public class UserFormFilesCreateDto
    {
        [Required(ErrorMessage = "files is required")]
        public List<FileUpload> Files { get; set; }
    }

    public class GetUserFormIdDto
    {
        public string UserFileId { get; set; }
        public string FileName { get; set; }
    }

    public class UserFormFileDetails
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public string Base64Content { get; set; }
    }
}
