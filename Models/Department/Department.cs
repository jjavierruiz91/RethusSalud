using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepartmentId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }

        public Country? Country { get; set; }
        public ICollection<City>? City { get; set; }
    }
}
