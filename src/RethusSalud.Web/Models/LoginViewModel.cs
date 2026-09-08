using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ingresa tu correo electronico")]
    [EmailAddress(ErrorMessage = "Correo electronico invalido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa tu contrasena")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
