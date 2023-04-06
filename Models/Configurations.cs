using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rethus_backend.Models
{
  public class Configurations
  {
    public string? ConfigurationsId { get; set; }

    public string? state { get; set; }

    public string? step { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? UserId { get; set; }

    public User? User { get; set; }
  }
}