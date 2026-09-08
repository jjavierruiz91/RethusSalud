using RethusSalud.Domain.Entities;

namespace RethusSalud.Web.Models.Admin;

public class SeguimientoBandejaViewModel
{
    public SeguimientoFiltroViewModel Filtro { get; set; } = new();
    public List<Solicitud> Solicitudes { get; set; } = new();
}
