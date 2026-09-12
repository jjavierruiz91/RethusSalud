using System.ComponentModel.DataAnnotations;
using RethusSalud.Application.Dtos;

namespace RethusSalud.Web.Models;

public class ConsultaEstadoViewModel
{
    [Required(ErrorMessage = "Ingresa tu numero de identificacion")]
    [Display(Name = "Numero de identificacion")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "El numero de identificacion debe tener entre {2} y {1} caracteres")]
    [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "El numero de identificacion solo puede contener letras y numeros")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    public bool Consultado { get; set; }
    public ConsultaEstadoDto? Resultado { get; set; }
}
