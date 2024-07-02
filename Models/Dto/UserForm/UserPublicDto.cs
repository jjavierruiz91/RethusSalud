using System.ComponentModel.DataAnnotations;

namespace rethus_backend.Models.Dto.UserPublic
{
    public class UserRestorePassword
    {
        [Required]
        public required string email { get; set; }
    }

    public class UserUpdatePassword
    {
        [Required]
        public required string password { get; set; }

        [Required]
        public required string token { get; set; }
    }

    public class UserPayloadPassword
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required string newPassword { get; set; }
    }

    public class TemplateConfigurationDto
    {
        public string TemplatePashEmail { get; set; }
        public string Token { get; set; }
    }

    public class RestoreSendEmailUser
    {
        public string Token { get; set; }
    }
}
