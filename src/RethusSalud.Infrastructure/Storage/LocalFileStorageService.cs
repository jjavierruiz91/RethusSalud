using Microsoft.Extensions.Configuration;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Infrastructure.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        var configurado = configuration["Storage:BasePath"];
        _basePath = string.IsNullOrWhiteSpace(configurado)
            ? Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "uploads")
            : configurado;
    }

    public async Task<string> GuardarArchivoAsync(int solicitudId, TipoDocumentoAdjunto tipo, string nombreArchivo, Stream contenido)
    {
        var carpeta = Path.Combine(_basePath, solicitudId.ToString());
        Directory.CreateDirectory(carpeta);

        var extension = Path.GetExtension(nombreArchivo);
        var rutaCompleta = Path.Combine(carpeta, $"{tipo}{extension}");

        await using (var destino = File.Create(rutaCompleta))
        {
            await contenido.CopyToAsync(destino);
        }

        return rutaCompleta;
    }

    public Task<Stream> AbrirArchivoAsync(string rutaAlmacenamiento)
    {
        Stream stream = File.OpenRead(rutaAlmacenamiento);
        return Task.FromResult(stream);
    }
}
