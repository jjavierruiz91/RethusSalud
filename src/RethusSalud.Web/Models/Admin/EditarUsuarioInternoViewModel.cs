using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RethusSalud.Web.Models.Admin;

public class EditarUsuarioInternoViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el correo electronico")]
    [EmailAddress(ErrorMessage = "Correo electronico invalido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona un rol")]
    public string Rol { get; set; } = string.Empty;

    public IFormFile? Foto { get; set; }
}
