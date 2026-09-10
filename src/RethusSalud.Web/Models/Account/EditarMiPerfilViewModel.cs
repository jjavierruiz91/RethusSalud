using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RethusSalud.Web.Models.Account;

public class EditarMiPerfilViewModel
{
    [Required(ErrorMessage = "Ingresa tu nombre")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa tu correo electronico")]
    [EmailAddress(ErrorMessage = "Correo electronico invalido")]
    public string Email { get; set; } = string.Empty;

    public string? NumeroIdentificacion { get; set; }

    [Phone(ErrorMessage = "Numero de telefono invalido")]
    public string? Telefono { get; set; }

    public IFormFile? Foto { get; set; }
}
