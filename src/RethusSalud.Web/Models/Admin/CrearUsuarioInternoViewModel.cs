using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Web.Models.Admin;

public class CrearUsuarioInternoViewModel
{
    [Required(ErrorMessage = "Ingresa el nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el correo electronico")]
    [EmailAddress(ErrorMessage = "Correo electronico invalido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona un rol")]
    public string Rol { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa una contrasena")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "La contrasena debe tener al menos 8 caracteres")]
    public string Password { get; set; } = string.Empty;
}
