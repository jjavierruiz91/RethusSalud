using System.ComponentModel.DataAnnotations;

namespace RethusSalud.Web.Models;

public class ConsultaEstadoViewModel
{
    [Required(ErrorMessage = "Ingresa tu numero de identificacion")]
    [Display(Name = "Numero de identificacion")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    public string? Resultado { get; set; }
}
