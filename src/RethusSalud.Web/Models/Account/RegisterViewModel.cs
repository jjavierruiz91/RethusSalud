using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Web.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ingresa tu nombre")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa tu correo electronico")]
    [EmailAddress(ErrorMessage = "Correo electronico invalido")]
    [Display(Name = "Correo electronico")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa tu numero de identificacion")]
    [Display(Name = "Identificacion")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa tu numero de telefono")]
    [Phone(ErrorMessage = "Numero de telefono invalido")]
    [Display(Name = "Telefono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa una contrasena")]
    [DataType(DataType.Password)]
    [MinLength(4, ErrorMessage = "La contrasena debe tener al menos 4 caracteres")]
    [Display(Name = "Contrasena")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma tu contrasena")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Las contrasenas no coinciden")]
    [Display(Name = "Confirmar contrasena")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}
