using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }

        public string? email { get; set; }

        [Required]
        public required string Identification { get; set; }

        [Required]
        public required string name { get; set; }

        [Required]
        public required byte[]? PasswordHash { get; set; }

        [Required]
        public required byte[]? PasswordSalt { get; set; }

        [Required]
        public required UserStatus? Status { get; set; }

        [Required]
        public required string roles { get; set; }
        public string? Token { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Configurations Configurations { get; set; }

        public UserForm UserForm { get; set; }

        public virtual UserDigitalSignature? DigitalSignature { get; set; }
    }
}
