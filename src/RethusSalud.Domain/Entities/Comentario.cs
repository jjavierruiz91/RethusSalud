using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Domain.Entities;

public class Comentario : Entity
{
    public int SolicitudId { get; set; }
    public Solicitud Solicitud { get; set; } = null!;

    public string AutorUserId { get; set; } = string.Empty;
    public string AutorNombre { get; set; } = string.Empty;
    public string? AutorFotoUrl { get; set; }
    public EtapaSolicitud? Etapa { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
