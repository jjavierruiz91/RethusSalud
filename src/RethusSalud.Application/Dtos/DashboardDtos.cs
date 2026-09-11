using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Dtos;

public class DashboardFiltroDto
{
    public EtapaSolicitud? Etapa { get; set; }
    public int? PaisId { get; set; }
    public int? DepartamentoId { get; set; }
    public int? MunicipioId { get; set; }
    public Genero? Genero { get; set; }
    public GrupoEtnico? GrupoEtnico { get; set; }
    public OrigenTitulo? OrigenTitulo { get; set; }
    public TipoInstitucion? TipoInstitucion { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
}

public record EtapaConteoDto(EtapaSolicitud Etapa, int Cantidad);
public record PaisConteoDto(int? PaisId, string Pais, int Cantidad);
public record DepartamentoConteoDto(int? DepartamentoId, string Departamento, int Cantidad);
public record MunicipioConteoDto(int? MunicipioId, string Municipio, int Cantidad);
public record GeneroConteoDto(Genero Genero, int Cantidad);
public record GrupoEtnicoConteoDto(GrupoEtnico GrupoEtnico, int Cantidad);
public record OrigenTituloConteoDto(OrigenTitulo Origen, int Cantidad);
public record TipoInstitucionConteoDto(TipoInstitucion Tipo, int Cantidad);
public record TipoDocumentoConteoDto(TipoDocumentoAdjunto Tipo, int Cantidad);
public record ProfesionConteoDto(int ProfesionId, string Profesion, int Cantidad);
public record RangoEdadConteoDto(string Rango, int Orden, int Cantidad);
public record TendenciaMesDto(int Anio, int Mes, int Cantidad);
public record FuncionarioDesempenoDto(string UsuarioId, string Nombre, EtapaSolicitud? Etapa, int Gestionadas, int Aprobadas, int Rechazadas);
public record SolicitudResumenDto(int Id, string NumeroIdentificacion, string NombreCompleto, string Profesion, EstadoSolicitud Estado, EtapaSolicitud? Etapa, DateTime FechaCreacion);

public class DashboardResultDto
{
    public int Total { get; set; }
    public int EnProceso { get; set; }
    public int Aprobadas { get; set; }
    public int Rechazadas { get; set; }
    public int EsperandoLargo { get; set; }
    public double? TiempoPromedioAprobacionDias { get; set; }

    public List<EtapaConteoDto> PorEtapa { get; set; } = new();
    public List<PaisConteoDto> PorPais { get; set; } = new();
    public List<DepartamentoConteoDto> PorDepartamento { get; set; } = new();
    public List<MunicipioConteoDto> PorMunicipio { get; set; } = new();
    public List<GeneroConteoDto> PorGenero { get; set; } = new();
    public List<GrupoEtnicoConteoDto> PorGrupoEtnico { get; set; } = new();
    public List<OrigenTituloConteoDto> PorOrigenTitulo { get; set; } = new();
    public List<TipoInstitucionConteoDto> PorTipoInstitucion { get; set; } = new();
    public List<TipoDocumentoConteoDto> PorTipoDocumento { get; set; } = new();
    public List<ProfesionConteoDto> TopProfesiones { get; set; } = new();
    public List<RangoEdadConteoDto> PorRangoEdad { get; set; } = new();
    public List<TendenciaMesDto> TendenciaMensual { get; set; } = new();
    public List<FuncionarioDesempenoDto> DesempenoFuncionarios { get; set; } = new();
    public List<SolicitudResumenDto> SolicitudesRecientes { get; set; } = new();
}
