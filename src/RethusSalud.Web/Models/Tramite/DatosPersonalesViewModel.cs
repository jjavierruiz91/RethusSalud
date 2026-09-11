using System.ComponentModel.DataAnnotations;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Tramite;

public class DatosPersonalesViewModel
{
    public int SolicitudId { get; set; }

    [Required, Display(Name = "Tipo de identificacion")]
    public TipoIdentificacion TipoIdentificacion { get; set; }

    [Required, Display(Name = "Numero de identificacion")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required, Display(Name = "Lugar de expedicion")]
    public string LugarExpedicion { get; set; } = string.Empty;

    [Required]
    public Genero Genero { get; set; }

    [Required]
    public string Nombres { get; set; } = string.Empty;

    [Required]
    public string Apellidos { get; set; } = string.Empty;

    [Required, Display(Name = "Pais de nacimiento")]
    public int PaisNacimientoId { get; set; }

    [Display(Name = "Departamento de nacimiento")]
    public int? DepartamentoNacimientoId { get; set; }

    [Display(Name = "Municipio de nacimiento")]
    public int? MunicipioNacimientoId { get; set; }

    [Display(Name = "Departamento o provincia de nacimiento")]
    public string? DepartamentoNacimientoTexto { get; set; }

    [Display(Name = "Municipio o ciudad de nacimiento")]
    public string? MunicipioNacimientoTexto { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Fecha de nacimiento")]
    public DateOnly FechaNacimiento { get; set; }

    [Required, Display(Name = "Pais de residencia")]
    public int PaisResidenciaId { get; set; }

    [Required(ErrorMessage = "El campo Departamento de residencia es obligatorio."), Display(Name = "Departamento de residencia")]
    public int? DepartamentoResidenciaId { get; set; }

    [Required(ErrorMessage = "El campo Municipio de residencia es obligatorio."), Display(Name = "Municipio de residencia")]
    public int? MunicipioResidenciaId { get; set; }

    [Required, Display(Name = "Direccion de domicilio")]
    public string DireccionDomicilio { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo Telefono fijo es obligatorio."), Display(Name = "Telefono fijo")]
    public string? TelefonoFijo { get; set; }

    [Required]
    public string Celular { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "Correo electronico")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo Grupo etnico es obligatorio."), Display(Name = "Grupo etnico")]
    public GrupoEtnico? GrupoEtnico { get; set; }
}
