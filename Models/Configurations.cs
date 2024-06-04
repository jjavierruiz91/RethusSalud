using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
    public class Configurations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ConfigurationsId { get; set; }

        public required string State { get; set; }

        public required string Step { get; set; }
        public required string TypeProcedure { get; set; }
        public required Boolean TermCondition { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
