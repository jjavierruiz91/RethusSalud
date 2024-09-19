using System.ComponentModel.DataAnnotations;

namespace rethus_backend.Models.Dto.UserForm
{
    public class UserFormConsecutiveDto
    {
        [Required]
        public required string consecutive { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public required string consecutiveDate { get; set; }
    }

    public class UserFormConsecutiveResponseDto
    {
        public string? Consecutive { get; set; }
        public string? ConsecutiveDate { get; set; }
    }
}
