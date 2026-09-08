using RethusSalud.Domain.Common;

namespace RethusSalud.Domain.Entities;

public class Comentario : Entity
{
    public int SolicitudId { get; set; }
    public Solicitud Solicitud { get; set; } = null!;

    public string AutorUserId { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
