using System.ComponentModel.DataAnnotations;

namespace rethus_backend.Models.Dto.UserForm
{
    public class UserFormConsecutiveDto
    {
        [Required]
        public required string consecutive { get; set; }
    }
}
