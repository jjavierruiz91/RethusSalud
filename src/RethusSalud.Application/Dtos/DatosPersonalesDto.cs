using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Dtos;

public class DatosPersonalesDto
{
    public TipoIdentificacion TipoIdentificacion { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string LugarExpedicion { get; set; } = string.Empty;
    public Genero Genero { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public int PaisNacimientoId { get; set; }
    public int? DepartamentoNacimientoId { get; set; }
    public int? MunicipioNacimientoId { get; set; }
    public string? DepartamentoNacimientoTexto { get; set; }
    public string? MunicipioNacimientoTexto { get; set; }
    public DateOnly FechaNacimiento { get; set; }

    public int PaisResidenciaId { get; set; }
    public int? DepartamentoResidenciaId { get; set; }
    public int? MunicipioResidenciaId { get; set; }

    public string DireccionDomicilio { get; set; } = string.Empty;
    public string? TelefonoFijo { get; set; }
    public string Celular { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public GrupoEtnico GrupoEtnico { get; set; }
}
