using System.ComponentModel.DataAnnotations;

namespace rethus_backend.Models.Dto.UserPublic
{
    public class UserRestorePassword
    {
        [Required]
        public required string email { get; set; }
    }
}
