using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Dashboard;

public class DashboardFiltroViewModel
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

    public DashboardFiltroDto ToDto() => new()
    {
        Etapa = Etapa,
        PaisId = PaisId,
        DepartamentoId = DepartamentoId,
        MunicipioId = MunicipioId,
        Genero = Genero,
        GrupoEtnico = GrupoEtnico,
        OrigenTitulo = OrigenTitulo,
        TipoInstitucion = TipoInstitucion,
        Desde = Desde,
        Hasta = Hasta
    };
}
