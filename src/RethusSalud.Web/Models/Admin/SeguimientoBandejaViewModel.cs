using RethusSalud.Application.Common;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Web.Models.Admin;

public class SeguimientoBandejaViewModel
{
    public SeguimientoFiltroViewModel Filtro { get; set; } = new();
    public PagedResult<Solicitud> Pagina { get; set; } = new();
    public int TotalEnProceso { get; set; }
    public int TotalAprobadas { get; set; }
    public int TotalRechazadas { get; set; }
}
