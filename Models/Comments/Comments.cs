using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using rethus_backend.Utilities.Constants.User.CommentsConstants;

namespace rethus_backend.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CommentId { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [ForeignKey("Users")]
        public string UserId { get; set; }

        [Required]
        [ForeignKey("UsersForm")]
        public string UserFormId { get; set; }

        [Required]
        public required CommentsStatus Status { get; set; } // Pendiente, Aprobada, Rechazada

        [Required]
        public required CommentsType Type { get; set; } // Comentario General, Aprobación, Rechazo

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}
