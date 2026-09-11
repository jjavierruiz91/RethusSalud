using RethusSalud.Application.Common;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Services;

public class DocumentoService
{
    private static readonly string[] TiposContenidoPermitidos = { "application/pdf", "image/jpeg", "image/png" };
    private const long TamanoMaximoBytes = 10 * 1024 * 1024;

    private static readonly Dictionary<string, byte[][]> FirmasPorTipo = new()
    {
        ["application/pdf"] = new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } }, // %PDF
        ["image/jpeg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
        ["image/png"] = new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } }
    };

    private readonly ISolicitudRepository _solicitudes;
    private readonly IFileStorageService _storage;
    private readonly IEmailSender _emailSender;

    public DocumentoService(ISolicitudRepository solicitudes, IFileStorageService storage, IEmailSender emailSender)
    {
        _solicitudes = solicitudes;
        _storage = storage;
        _emailSender = emailSender;
    }

    public static List<TipoDocumentoAdjunto> DocumentosRequeridos(Solicitud solicitud)
    {
        var requeridos = new List<TipoDocumentoAdjunto>
        {
            TipoDocumentoAdjunto.CedulaAmpliada,
            TipoDocumentoAdjunto.DiplomaGrado,
            TipoDocumentoAdjunto.ActaGrado
        };

        var esPsicologiaPorProfesion = solicitud.Profesion.Nombre.Contains("Psicologia", StringComparison.OrdinalIgnoreCase);
        var esPsicologiaPorPrograma = solicitud.DatosAcademicos?.NombrePrograma?.Contains("Psicolog", StringComparison.OrdinalIgnoreCase) ?? false;

        if (esPsicologiaPorProfesion || esPsicologiaPorPrograma)
        {
            requeridos.Add(TipoDocumentoAdjunto.TarjetaProfesional);
        }

        return requeridos;
    }

    public async Task CargarDocumentoAsync(
        int solicitudId,
        TipoDocumentoAdjunto tipo,
        string nombreArchivo,
        string contentType,
        long tamanoBytes,
        Stream contenido)
    {
        if (!TiposContenidoPermitidos.Contains(contentType))
        {
            throw new AppValidationException(new[] { "Solo se permiten archivos PDF, JPG o PNG." });
        }

        if (tamanoBytes <= 0 || tamanoBytes > TamanoMaximoBytes)
        {
            throw new AppValidationException(new[] { "El archivo debe pesar menos de 10 MB." });
        }

        if (!contenido.CanSeek)
        {
            throw new AppValidationException(new[] { "No se pudo validar el contenido del archivo." });
        }

        var encabezado = new byte[8];
        var leidos = await contenido.ReadAsync(encabezado.AsMemory(0, encabezado.Length));
        contenido.Position = 0;

        var firmasEsperadas = FirmasPorTipo[contentType];
        var coincide = firmasEsperadas.Any(firma => leidos >= firma.Length && firma.SequenceEqual(encabezado.Take(firma.Length)));
        if (!coincide)
        {
            throw new AppValidationException(new[] { "El contenido del archivo no coincide con el tipo declarado." });
        }

        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        var rutaAlmacenamiento = await _storage.GuardarArchivoAsync(solicitudId, tipo, nombreArchivo, contenido);

        var existente = solicitud.Archivos.FirstOrDefault(a => a.TipoDocumento == tipo);
        if (existente is not null)
        {
            solicitud.Archivos.Remove(existente);
        }

        solicitud.AdjuntarArchivo(new ArchivoAdjunto
        {
            TipoDocumento = tipo,
            NombreArchivo = nombreArchivo,
            RutaAlmacenamiento = rutaAlmacenamiento,
            ContentType = contentType,
            TamanoBytes = tamanoBytes,
            FechaCarga = DateTime.UtcNow
        });

        await _solicitudes.SaveChangesAsync();
    }

    public async Task<(ArchivoAdjunto Archivo, Stream Contenido)> AbrirArchivoAsync(int archivoId)
    {
        var archivo = await _solicitudes.GetArchivoByIdAsync(archivoId)
            ?? throw new InvalidOperationException("Archivo no encontrado.");

        var contenido = await _storage.AbrirArchivoAsync(archivo.RutaAlmacenamiento);
        return (archivo, contenido);
    }

    public async Task RadicarAsync(int solicitudId, string usuarioId)
    {
        var solicitud = await _solicitudes.GetByIdAsync(solicitudId)
            ?? throw new InvalidOperationException("Solicitud no encontrada.");

        var requeridos = DocumentosRequeridos(solicitud);
        var faltantes = requeridos.Where(r => solicitud.Archivos.All(a => a.TipoDocumento != r)).ToList();
        if (faltantes.Count > 0)
        {
            throw new AppValidationException(new[] { "Debes cargar todos los documentos requeridos antes de enviar la solicitud." });
        }

        solicitud.Radicar(usuarioId);
        await _solicitudes.SaveChangesAsync();

        await _emailSender.EnviarAsync(solicitud.Solicitante.CorreoElectronico, "Tu solicitud fue radicada",
            $"<p>Hola {solicitud.Solicitante.Nombres},</p><p>Tu solicitud de {solicitud.TipoTramite} fue radicada exitosamente y sera revisada por los funcionarios de la Secretaria de salud del Cesar.</p>");
    }
}
