using System.ComponentModel.DataAnnotations;
using rethus_backend.Models;

namespace rethus_backend.Models.Dto.User
{
    public class CreateUserDigitalSignatureDto
    {
        [Required]
        [StringLength(
            100,
            ErrorMessage = "El nombre de la firma no puede superar los 100 caracteres."
        )]
        public string SignatureName { get; set; }

        [Required]
        [EnumDataType(typeof(SignatureType), ErrorMessage = "El tipo de firma no es válido.")]
        public SignatureType SignatureType { get; set; }

        [Required]
        public IFormFile SignatureImage { get; set; }
    }

    public class UpdateUserDigitalSignatureDto
    {
        [Required]
        [StringLength(
            100,
            ErrorMessage = "El nombre de la firma no puede superar los 100 caracteres."
        )]
        public string SignatureName { get; set; }

        [Required]
        [EnumDataType(typeof(SignatureType), ErrorMessage = "El tipo de firma no es válido.")]
        public SignatureType SignatureType { get; set; }

        [Required]
        [EnumDataType(typeof(SignatureStatus), ErrorMessage = "El tipo de status no es válido.")]
        public SignatureStatus SignatureStatus { get; set; }

        public IFormFile? SignatureImage { get; set; }
    }
}
