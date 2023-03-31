using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
  public class User
  {

    public string? email { get; set; }

    [Key]
    public string? identification { get; set; }
    public string? status { get; set; }
    public string? Roles { get; set; }
    public string? Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}