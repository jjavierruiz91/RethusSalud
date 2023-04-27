using System.ComponentModel.DataAnnotations;
namespace rethus_backend.Models.Dto.UserFormFiles
{
  public class UserFormFilesCreateDto
  {
    [Required(ErrorMessage = "files is required")]
    public List<IFormFile> files { get; set; }
  }
}