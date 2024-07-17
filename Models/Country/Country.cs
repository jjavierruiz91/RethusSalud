using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CountryId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Code { get; set; }

        [Required]
        public required string Capital { get; set; }

        [Required]
        public required string Region { get; set; }

        [Required]
        public required string Flag { get; set; }

        [Required]
        public required string DiallingCode { get; set; }

        [Required]
        public required string IsoCode { get; set; }

        [Required]
        public required int Population { get; set; }

        public ICollection<Department>? Departments { get; set; }
    }
}
