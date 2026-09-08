using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Domain.Entities;

public class Solicitante : Entity
{
    public string ApplicationUserId { get; set; } = string.Empty;

    public TipoIdentificacion TipoIdentificacion { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string LugarExpedicion { get; set; } = string.Empty;
    public Genero Genero { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public int PaisNacimientoId { get; set; }
    public Pais PaisNacimiento { get; set; } = null!;
    public int? DepartamentoNacimientoId { get; set; }
    public Departamento? DepartamentoNacimiento { get; set; }
    public int? MunicipioNacimientoId { get; set; }
    public Municipio? MunicipioNacimiento { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public int PaisResidenciaId { get; set; }
    public Pais PaisResidencia { get; set; } = null!;
    public int? DepartamentoResidenciaId { get; set; }
    public Departamento? DepartamentoResidencia { get; set; }
    public int? MunicipioResidenciaId { get; set; }
    public Municipio? MunicipioResidencia { get; set; }

    public string DireccionDomicilio { get; set; } = string.Empty;
    public string? TelefonoFijo { get; set; }
    public string Celular { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public GrupoEtnico GrupoEtnico { get; set; }
    public DateTime? FechaAceptacionTerminos { get; set; }

    public ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();

    public void AceptarTerminos()
    {
        FechaAceptacionTerminos = DateTime.UtcNow;
    }
}
