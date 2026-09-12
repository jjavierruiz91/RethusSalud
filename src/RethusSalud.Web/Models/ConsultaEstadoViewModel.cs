using System.ComponentModel.DataAnnotations;
using RethusSalud.Application.Dtos;

namespace RethusSalud.Web.Models;

public class ConsultaEstadoViewModel
{
    [Required(ErrorMessage = "Ingresa tu numero de identificacion")]
    [Display(Name = "Numero de identificacion")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    public bool Consultado { get; set; }
    public ConsultaEstadoDto? Resultado { get; set; }
}
