using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [ForeignKey("Country")]
        public string CountryId { get; set; }
    }
}
