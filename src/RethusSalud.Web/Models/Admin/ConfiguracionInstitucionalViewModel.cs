using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RethusSalud.Web.Models.Admin;

public class ConfiguracionInstitucionalViewModel
{
    [Required(ErrorMessage = "Ingresa el nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el cargo")]
    public string Cargo { get; set; } = string.Empty;

    public string? FirmaUrl { get; set; }

    public IFormFile? Firma { get; set; }

    public List<UsuarioInternoViewModel> Funcionarios { get; set; } = new();
}
