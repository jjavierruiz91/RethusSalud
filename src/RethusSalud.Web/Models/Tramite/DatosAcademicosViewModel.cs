using System.ComponentModel.DataAnnotations;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Tramite;

public class DatosAcademicosViewModel
{
    public int SolicitudId { get; set; }

    [Required, Display(Name = "Origen del titulo")]
    public OrigenTitulo OrigenTitulo { get; set; }

    [Required, Display(Name = "Tipo de institucion")]
    public TipoInstitucion TipoInstitucion { get; set; }

    [Required, Display(Name = "Tipo de programa")]
    public string TipoPrograma { get; set; } = string.Empty;

    [Required, Display(Name = "Pais de institucion educativa")]
    public int PaisInstitucionId { get; set; }

    [Display(Name = "Departamento de institucion educativa")]
    public int? DepartamentoInstitucionId { get; set; }

    [Display(Name = "Municipio de institucion educativa")]
    public int? MunicipioInstitucionId { get; set; }

    [Required, Display(Name = "Nombre de institucion educativa")]
    public string NombreInstitucion { get; set; } = string.Empty;

    [Required, Display(Name = "Nombre del programa")]
    public string NombrePrograma { get; set; } = string.Empty;

    [Required, DataType(DataType.Date), Display(Name = "Fecha de grado")]
    public DateOnly FechaGrado { get; set; }

    [Display(Name = "Numero de convalidacion")]
    public string? NumeroConvalidacion { get; set; }

    [DataType(DataType.Date), Display(Name = "Fecha de convalidacion")]
    public DateOnly? FechaConvalidacion { get; set; }

    [Display(Name = "Titulo equivalente")]
    public string? TituloEquivalente { get; set; }
}
