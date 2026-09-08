using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Domain.Entities;

public class Consecutivo : Entity
{
    public int SolicitudId { get; set; }
    public Solicitud Solicitud { get; set; } = null!;

    public string Numero { get; set; } = string.Empty;
    public DateOnly Fecha { get; set; }
    public ModoConsecutivo Modo { get; set; }
}
