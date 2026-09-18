using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Revision;

public class BandejaFiltroViewModel
{
    public string? NumeroIdentificacion { get; set; }
    public TipoTramite? TipoTramite { get; set; }

    // Puede ser un nombre de EstadoSolicitud (ej. "Rechazado") o el pseudo-estado "Reenviado"
    // (solicitud En proceso que tiene un rechazo previo en su historial).
    public string? Estado { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public int Pagina { get; set; } = 1;
}
