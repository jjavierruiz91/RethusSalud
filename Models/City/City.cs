using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
    public class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CityId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
    }
}
