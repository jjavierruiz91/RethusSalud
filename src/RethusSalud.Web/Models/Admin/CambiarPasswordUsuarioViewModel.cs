using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Web.Models.Admin;

public class CambiarPasswordUsuarioViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa la nueva contrasena")]
    [DataType(DataType.Password)]
    [MinLength(4, ErrorMessage = "La contrasena debe tener al menos 4 caracteres")]
    public string NuevaPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma la nueva contrasena")]
    [DataType(DataType.Password)]
    [Compare(nameof(NuevaPassword), ErrorMessage = "Las contrasenas no coinciden")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}
