using Moq;
using RethusSalud.Application.Common;
using RethusSalud.Application.Interfaces;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using Xunit;

namespace RethusSalud.Application.Tests.Services;

public class DocumentoServiceTests
{
    private static Profesion CrearProfesion(string nombre) => new()
    {
        Id = 1,
        Nombre = nombre,
        TipoTramite = TipoTramite.Rethus,
        NivelFormacion = NivelFormacion.Profesional
    };

    [Fact]
    public void DocumentosRequeridos_incluye_tarjeta_profesional_solo_para_psicologia()
    {
        var psicologia = DocumentoService.DocumentosRequeridos(CrearProfesion("Psicologia"));
        var auxiliar = DocumentoService.DocumentosRequeridos(CrearProfesion("Auxiliar en enfermeria"));

        Assert.Contains(TipoDocumentoAdjunto.TarjetaProfesional, psicologia);
        Assert.DoesNotContain(TipoDocumentoAdjunto.TarjetaProfesional, auxiliar);
    }

    [Fact]
    public async Task CargarDocumentoAsync_rechaza_contenido_que_no_coincide_con_el_tipo_declarado()
    {
        var solicitudes = new Mock<ISolicitudRepository>();
        var storage = new Mock<IFileStorageService>();
        var emailSender = new Mock<IEmailSender>();
        var servicio = new DocumentoService(solicitudes.Object, storage.Object, emailSender.Object);

        var contenidoFalso = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("esto no es un pdf"));

        await Assert.ThrowsAsync<AppValidationException>(() =>
            servicio.CargarDocumentoAsync(1, TipoDocumentoAdjunto.CedulaAmpliada, "cedula.pdf", "application/pdf", contenidoFalso.Length, contenidoFalso));
    }

    [Fact]
    public async Task RadicarAsync_lanza_excepcion_si_faltan_documentos_requeridos()
    {
        var solicitante = new Solicitante { Id = 1, Nombres = "Franco", Apellidos = "Ovalle", CorreoElectronico = "franco@example.com" };
        var profesion = CrearProfesion("Psicologia");
        var solicitud = Solicitud.IniciarBorrador(solicitante, profesion);
        solicitud.DatosAcademicos = new DatosAcademicos { NombreInstitucion = "Universidad de prueba" };
        // No se adjunta ningun archivo: deben faltar todos los requeridos.

        var solicitudes = new Mock<ISolicitudRepository>();
        solicitudes.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(solicitud);
        var storage = new Mock<IFileStorageService>();
        var emailSender = new Mock<IEmailSender>();
        var servicio = new DocumentoService(solicitudes.Object, storage.Object, emailSender.Object);

        await Assert.ThrowsAsync<AppValidationException>(() => servicio.RadicarAsync(1, "usuario-1"));
    }
}
