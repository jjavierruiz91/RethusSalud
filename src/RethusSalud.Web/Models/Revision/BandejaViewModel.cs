using RethusSalud.Application.Common;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Revision;

public class BandejaViewModel
{
    public EtapaSolicitud Etapa { get; set; }
    public BandejaFiltroViewModel Filtro { get; set; } = new();
    public PagedResult<Solicitud> Pagina { get; set; } = new();
    public int TotalEnProceso { get; set; }
    public int TotalAprobadas { get; set; }
    public int TotalEsperandoLargo { get; set; }
}
