using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Admin;

public class SeguimientoFiltroViewModel
{
    public string? NumeroIdentificacion { get; set; }
    public TipoTramite? TipoTramite { get; set; }
    public EtapaSolicitud? Etapa { get; set; }
    public EstadoSolicitud? Estado { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public int Pagina { get; set; } = 1;
}
