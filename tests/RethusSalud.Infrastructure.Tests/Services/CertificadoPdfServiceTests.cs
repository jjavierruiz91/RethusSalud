using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Services;
using Xunit;

namespace RethusSalud.Infrastructure.Tests.Services;

public class CertificadoPdfServiceTests
{
    private static IConfiguration CrearConfiguracion() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["DomainWebUrl"] = "https://localhost:5299" })
            .Build();

    private static IWebHostEnvironment CrearAmbiente() => new AmbientePrueba();

    private static readonly FirmantesDocumentoDto FirmantesVacios = new(null, null, null, null, null);

    private class AmbientePrueba : IWebHostEnvironment
    {
        public string WebRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string ApplicationName { get; set; } = "Tests";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public string EnvironmentName { get; set; } = "Test";
    }

    private static Solicitud CrearSolicitudAprobada()
    {
        var pais = new Pais { Id = 1, Nombre = "Colombia" };
        var solicitante = new Solicitante
        {
            Id = 1,
            Nombres = "Franco",
            Apellidos = "Ovalle",
            NumeroIdentificacion = "123456789",
            TipoIdentificacion = TipoIdentificacion.CedulaCiudadania,
            PaisNacimiento = pais,
            PaisResidencia = pais
        };
        var profesion = new Profesion { Id = 1, Nombre = "Psicologia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Profesional };

        var solicitud = Solicitud.IniciarBorrador(solicitante, profesion);
        solicitud.DatosAcademicos = new DatosAcademicos
        {
            NombreInstitucion = "Universidad de prueba",
            FechaGrado = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
            PaisInstitucion = pais
        };
        solicitud.Radicar("solicitante-1");
        solicitud.Aprobar("f1");
        solicitud.Aprobar("f2");
        solicitud.Aprobar("f3");
        solicitud.AsignarConsecutivo("RTH-2026-000001", ModoConsecutivo.Manual);
        solicitud.Aprobar("inventario");

        return solicitud;
    }

    [Fact]
    public void Generar_produce_un_pdf_valido_para_una_solicitud_aprobada()
    {
        var servicio = new CertificadoPdfService(CrearConfiguracion(), CrearAmbiente());
        var solicitud = CrearSolicitudAprobada();

        var pdf = servicio.Generar(solicitud, FirmantesVacios);

        Assert.NotEmpty(pdf);
        Assert.Equal("%PDF", System.Text.Encoding.ASCII.GetString(pdf, 0, 4));
    }

    [Fact]
    public void Generar_lanza_excepcion_si_no_hay_consecutivo_asignado()
    {
        var pais = new Pais { Id = 1, Nombre = "Colombia" };
        var solicitante = new Solicitante { Id = 1, Nombres = "Franco", Apellidos = "Ovalle", PaisNacimiento = pais, PaisResidencia = pais };
        var profesion = new Profesion { Id = 1, Nombre = "Psicologia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Profesional };
        var solicitud = Solicitud.IniciarBorrador(solicitante, profesion);

        var servicio = new CertificadoPdfService(CrearConfiguracion(), CrearAmbiente());

        Assert.Throws<InvalidOperationException>(() => servicio.Generar(solicitud, FirmantesVacios));
    }
}
