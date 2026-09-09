using RethusSalud.Domain.Entities;

namespace RethusSalud.Web.Models.Revision;

public class BuscarViewModel
{
    public BuscarFiltroViewModel Filtro { get; set; } = new();
    public List<Solicitud> Solicitudes { get; set; } = new();
}
