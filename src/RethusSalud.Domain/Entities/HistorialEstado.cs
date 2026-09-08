using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Domain.Entities;

public class HistorialEstado : Entity
{
    public int SolicitudId { get; set; }
    public Solicitud Solicitud { get; set; } = null!;

    public EstadoSolicitud EstadoResultante { get; set; }
    public EtapaSolicitud? EtapaResultante { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public string? Motivo { get; set; }
    public DateTime Fecha { get; set; }
}
