using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> GuardarArchivoAsync(int solicitudId, TipoDocumentoAdjunto tipo, string nombreArchivo, Stream contenido);
    Task<Stream> AbrirArchivoAsync(string rutaAlmacenamiento);
}
