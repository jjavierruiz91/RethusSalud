using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Revision;

public class BandejaViewModel
{
    public EtapaSolicitud Etapa { get; set; }
    public BandejaFiltroViewModel Filtro { get; set; } = new();
    public List<Solicitud> Solicitudes { get; set; } = new();
}
