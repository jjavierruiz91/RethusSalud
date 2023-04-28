using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
  public class UserFormFiles
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? UserFormFilesId { get; set; }
    public long? size { get; set; }
    public string? filename { get; set; }
    public string? type { get; set; }
    public string? url { get; set; }
    public string? UserFormId { get; set; }
    public UserForm? UserForm { get; set; }
  }
}