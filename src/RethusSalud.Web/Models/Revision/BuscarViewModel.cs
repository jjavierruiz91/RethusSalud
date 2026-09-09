using RethusSalud.Application.Common;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Web.Models.Revision;

public class BuscarViewModel
{
    public BuscarFiltroViewModel Filtro { get; set; } = new();
    public PagedResult<Solicitud> Pagina { get; set; } = new();
}
