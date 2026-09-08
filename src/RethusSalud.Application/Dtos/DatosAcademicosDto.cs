using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Dtos;

public class DatosAcademicosDto
{
    public OrigenTitulo OrigenTitulo { get; set; }
    public TipoInstitucion TipoInstitucion { get; set; }
    public string TipoPrograma { get; set; } = string.Empty;

    public int PaisInstitucionId { get; set; }
    public int? DepartamentoInstitucionId { get; set; }
    public int? MunicipioInstitucionId { get; set; }

    public string NombreInstitucion { get; set; } = string.Empty;
    public string NombrePrograma { get; set; } = string.Empty;
    public DateOnly FechaGrado { get; set; }

    public string? NumeroConvalidacion { get; set; }
    public DateOnly? FechaConvalidacion { get; set; }
    public string? TituloEquivalente { get; set; }
}
