using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Domain.Entities;

public class ArchivoAdjunto : Entity
{
    public int SolicitudId { get; set; }
    public Solicitud Solicitud { get; set; } = null!;

    public TipoDocumentoAdjunto TipoDocumento { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string RutaAlmacenamiento { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public DateTime FechaCarga { get; set; }
}
