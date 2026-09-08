using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Domain.Entities;

public class DatosAcademicos : Entity
{
    public int SolicitudId { get; set; }
    public Solicitud Solicitud { get; set; } = null!;

    public OrigenTitulo OrigenTitulo { get; set; }
    public TipoInstitucion TipoInstitucion { get; set; }
    public string TipoPrograma { get; set; } = string.Empty;

    public int PaisInstitucionId { get; set; }
    public Pais PaisInstitucion { get; set; } = null!;
    public int? DepartamentoInstitucionId { get; set; }
    public Departamento? DepartamentoInstitucion { get; set; }
    public int? MunicipioInstitucionId { get; set; }
    public Municipio? MunicipioInstitucion { get; set; }

    public string NombreInstitucion { get; set; } = string.Empty;
    public string NombrePrograma { get; set; } = string.Empty;
    public DateOnly FechaGrado { get; set; }

    public string? NumeroConvalidacion { get; set; }
    public DateOnly? FechaConvalidacion { get; set; }
    public string? TituloEquivalente { get; set; }
}
