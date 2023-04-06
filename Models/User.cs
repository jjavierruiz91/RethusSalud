using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
  public class User
  {

    public string? UserId { get; set; }
    public string? email { get; set; }
    public string? status { get; set; }
    public string? Roles { get; set; }
    public string? Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Configurations? Configurations { get; set; }

    public UserForm? UserForm { get; set; }
  }
}