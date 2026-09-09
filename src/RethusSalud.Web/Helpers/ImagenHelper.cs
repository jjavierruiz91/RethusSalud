using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace RethusSalud.Web.Helpers;

public static class ImagenHelper
{
    private static readonly string[] TiposPermitidos = { "image/jpeg", "image/png" };
    private const long TamanoMaximoBytes = 5 * 1024 * 1024;

    public static string? Validar(IFormFile archivo)
    {
        if (!TiposPermitidos.Contains(archivo.ContentType))
        {
            return "El archivo debe ser una imagen JPG o PNG.";
        }

        if (archivo.Length > TamanoMaximoBytes)
        {
            return "El archivo debe pesar menos de 5 MB.";
        }

        return null;
    }

    public static async Task<string> GuardarAsync(IWebHostEnvironment environment, string carpetaRelativa, string nombreBase, IFormFile archivo)
    {
        var carpeta = Path.Combine(environment.WebRootPath, "img", carpetaRelativa);
        Directory.CreateDirectory(carpeta);

        var extension = archivo.ContentType == "image/png" ? ".png" : ".jpg";
        var nombreArchivo = $"{nombreBase}{extension}";
        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        await using (var destino = File.Create(rutaCompleta))
        {
            await archivo.CopyToAsync(destino);
        }

        return $"/img/{carpetaRelativa}/{nombreArchivo}?v={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
    }
}
