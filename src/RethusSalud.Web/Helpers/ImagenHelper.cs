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

    private const string PrefijoPngBase64 = "data:image/png;base64,";

    public static string? ValidarFirmaDibujada(string dataUrl)
    {
        if (string.IsNullOrWhiteSpace(dataUrl) || !dataUrl.StartsWith(PrefijoPngBase64, StringComparison.Ordinal))
        {
            return "La firma dibujada no es válida.";
        }

        var base64 = dataUrl[PrefijoPngBase64.Length..];
        var bytesAproximados = base64.Length * 3 / 4;
        if (bytesAproximados > TamanoMaximoBytes)
        {
            return "La firma dibujada pesa demasiado.";
        }

        try
        {
            Convert.FromBase64String(base64);
        }
        catch (FormatException)
        {
            return "La firma dibujada no es válida.";
        }

        return null;
    }

    public static async Task<string> GuardarFirmaDibujadaAsync(IWebHostEnvironment environment, string carpetaRelativa, string nombreBase, string dataUrl)
    {
        var carpeta = Path.Combine(environment.WebRootPath, "img", carpetaRelativa);
        Directory.CreateDirectory(carpeta);

        var datos = Convert.FromBase64String(dataUrl[PrefijoPngBase64.Length..]);
        var nombreArchivo = $"{nombreBase}.png";
        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        await File.WriteAllBytesAsync(rutaCompleta, datos);

        return $"/img/{carpetaRelativa}/{nombreArchivo}?v={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
    }
}
