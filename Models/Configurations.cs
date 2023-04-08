using System.ComponentModel.DataAnnotations.Schema;
namespace rethus_backend.Models
{
  public class Configurations
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? ConfigurationsId { get; set; }

    public string? state { get; set; }

    public string? step { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? UserId { get; set; }

    public User? User { get; set; }
  }
}