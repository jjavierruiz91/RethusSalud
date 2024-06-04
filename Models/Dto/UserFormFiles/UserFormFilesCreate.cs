using System.ComponentModel.DataAnnotations;

namespace rethus_backend.Models.Dto.UserFormFiles
{
    public class UserFormFilesCreateDto
    {
        [Required(ErrorMessage = "files is required")]
        public List<IFormFile> files { get; set; }
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
