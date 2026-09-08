using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Domain.Exceptions;
using Xunit;

namespace RethusSalud.Domain.Tests;

public class SolicitudTests
{
    private static Solicitud CrearBorrador()
    {
        var pais = new Pais { Id = 1, Nombre = "Colombia" };
        var solicitante = new Solicitante
        {
            Id = 1,
            ApplicationUserId = "user-1",
            NumeroIdentificacion = "123456789",
            Nombres = "Franco",
            Apellidos = "Ovalle",
            PaisNacimiento = pais,
            PaisNacimientoId = pais.Id,
            PaisResidencia = pais,
            PaisResidenciaId = pais.Id
        };
        var profesion = new Profesion { Id = 1, Nombre = "PSICOLOGIA", TipoTramite = TipoTramite.ReTHUS, NivelFormacion = NivelFormacion.Profesional };

        return Solicitud.IniciarBorrador(solicitante, profesion);
    }

    private static Solicitud CrearSolicitudRadicada()
    {
        var solicitud = CrearBorrador();
        solicitud.DatosAcademicos = new DatosAcademicos { NombreInstitucion = "Universidad de prueba" };
        solicitud.Radicar("solicitante-1");
        return solicitud;
    }

    [Fact]
    public void IniciarBorrador_deja_la_solicitud_en_estado_borrador_sin_etapa()
    {
        var solicitud = CrearBorrador();

        Assert.Equal(EstadoSolicitud.Borrador, solicitud.Estado);
        Assert.Null(solicitud.EtapaActual);
        Assert.Empty(solicitud.Historial);
    }

    [Fact]
    public void Radicar_sin_datos_academicos_lanza_excepcion()
    {
        var solicitud = CrearBorrador();

        Assert.Throws<DomainException>(() => solicitud.Radicar("solicitante-1"));
    }

    [Fact]
    public void Radicar_pasa_la_solicitud_a_en_proceso_etapa1()
    {
        var solicitud = CrearSolicitudRadicada();

        Assert.Equal(EstadoSolicitud.EnProceso, solicitud.Estado);
        Assert.Equal(EtapaSolicitud.Etapa1, solicitud.EtapaActual);
        Assert.Single(solicitud.Historial);
    }

    [Fact]
    public void No_se_puede_radicar_dos_veces()
    {
        var solicitud = CrearSolicitudRadicada();

        Assert.Throws<DomainException>(() => solicitud.Radicar("solicitante-1"));
    }

    [Fact]
    public void Aprobar_avanza_secuencialmente_por_las_etapas()
    {
        var solicitud = CrearSolicitudRadicada();

        solicitud.Aprobar("funcionario-1");
        Assert.Equal(EtapaSolicitud.Etapa2, solicitud.EtapaActual);

        solicitud.Aprobar("funcionario-2");
        Assert.Equal(EtapaSolicitud.Etapa3, solicitud.EtapaActual);

        solicitud.Aprobar("funcionario-3");
        Assert.Equal(EtapaSolicitud.Inventario, solicitud.EtapaActual);
        Assert.Equal(EstadoSolicitud.EnProceso, solicitud.Estado);
    }

    [Fact]
    public void Aprobar_en_inventario_sin_consecutivo_lanza_excepcion()
    {
        var solicitud = CrearSolicitudRadicada();
        solicitud.Aprobar("f1");
        solicitud.Aprobar("f2");
        solicitud.Aprobar("f3");

        Assert.Throws<DomainException>(() => solicitud.Aprobar("inventario"));
    }

    [Fact]
    public void Aprobar_en_inventario_con_consecutivo_finaliza_aprobada()
    {
        var solicitud = CrearSolicitudRadicada();
        solicitud.Aprobar("f1");
        solicitud.Aprobar("f2");
        solicitud.Aprobar("f3");

        solicitud.AsignarConsecutivo("RTH-2026-000001", ModoConsecutivo.Manual);
        solicitud.Aprobar("inventario");

        Assert.Equal(EstadoSolicitud.Aprobado, solicitud.Estado);
    }

    [Fact]
    public void No_se_puede_asignar_consecutivo_antes_de_llegar_a_inventario()
    {
        var solicitud = CrearSolicitudRadicada();

        Assert.Throws<DomainException>(() => solicitud.AsignarConsecutivo("RTH-2026-000001", ModoConsecutivo.Manual));
    }

    [Fact]
    public void No_se_puede_asignar_consecutivo_dos_veces()
    {
        var solicitud = CrearSolicitudRadicada();
        solicitud.Aprobar("f1");
        solicitud.Aprobar("f2");
        solicitud.Aprobar("f3");
        solicitud.AsignarConsecutivo("RTH-2026-000001", ModoConsecutivo.Manual);

        Assert.Throws<DomainException>(() => solicitud.AsignarConsecutivo("RTH-2026-000002", ModoConsecutivo.Manual));
    }

    [Fact]
    public void Rechazar_requiere_motivo()
    {
        var solicitud = CrearSolicitudRadicada();

        Assert.Throws<DomainException>(() => solicitud.Rechazar("funcionario-1", ""));
    }

    [Fact]
    public void Rechazar_deja_la_solicitud_en_estado_terminal()
    {
        var solicitud = CrearSolicitudRadicada();

        solicitud.Rechazar("funcionario-1", "Documentos ilegibles");

        Assert.Equal(EstadoSolicitud.Rechazado, solicitud.Estado);
        Assert.Throws<DomainException>(() => solicitud.Aprobar("funcionario-2"));
    }
}
