using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.CertificadosAprobados;

public class AprobadasFiltroViewModel
{
    public string? NumeroIdentificacion { get; set; }
    public TipoTramite? TipoTramite { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public int Pagina { get; set; } = 1;
}
