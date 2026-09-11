using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Dashboard;

public class DashboardViewModel
{
    public DashboardFiltroViewModel Filtro { get; set; } = new();
    public bool EsSuperAdmin { get; set; }
    public EtapaSolicitud? EtapaBloqueada { get; set; }
    public DashboardResultDto Resultado { get; set; } = new();
    public List<CatalogoItemDto> Paises { get; set; } = new();
    public List<CatalogoItemDto> Departamentos { get; set; } = new();
    public List<CatalogoItemDto> Municipios { get; set; } = new();
}
