using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Revision;

public class BandejaFiltroViewModel
{
    public string? NumeroIdentificacion { get; set; }
    public TipoTramite? TipoTramite { get; set; }
    public EstadoSolicitud? Estado { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
}
