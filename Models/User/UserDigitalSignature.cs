using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace rethus_backend.Models
{
    public class UserDigitalSignature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserDigitalSignatureId { get; set; }

        [Required]
        [MaxLength(255)]
        public string SignatureName { get; set; } = string.Empty;

        [Required]
        public string SignatureImagePath { get; set; } = string.Empty;

        [Required]
        public SignatureStatus Status { get; set; } = SignatureStatus.Active;

        [Required]
        [MaxLength(50)]
        public SignatureType SignatureType { get; set; }

        [Required]
        [MaxLength(100)]
        public string SignaturePositionType { get; set; } = string.Empty; // Nuevo campo agregado con "Signature" alante

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public String UserId { get; set; }
        public virtual User User { get; set; }
    }

    public enum SignatureType
    {
        Project, // Firma para proyectos
        Review, // Firma para revisiones
        Approve, // Firma para aprobaciones
        Secretary // Firma del secretario
    }

    public enum SignatureStatus
    {
        Active, // Puede firmar
        Inactive // No puede firmar
    }
}
