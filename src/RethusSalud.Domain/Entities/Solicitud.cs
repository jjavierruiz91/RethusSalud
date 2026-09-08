using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;
using RethusSalud.Domain.Exceptions;

namespace RethusSalud.Domain.Entities;

public class Solicitud : Entity
{
    public int SolicitanteId { get; set; }
    public Solicitante Solicitante { get; set; } = null!;

    public int ProfesionId { get; set; }
    public Profesion Profesion { get; set; } = null!;

    public TipoTramite TipoTramite { get; set; }

    public EstadoSolicitud Estado { get; private set; } = EstadoSolicitud.Borrador;
    public EtapaSolicitud? EtapaActual { get; private set; }

    public DatosAcademicos? DatosAcademicos { get; set; }
    public Consecutivo? Consecutivo { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public DateTime? FechaRadicacion { get; set; }

    public ICollection<ArchivoAdjunto> Archivos { get; set; } = new List<ArchivoAdjunto>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    public ICollection<HistorialEstado> Historial { get; set; } = new List<HistorialEstado>();

    private Solicitud()
    {
        // Requerido por EF Core.
    }

    public static Solicitud IniciarBorrador(Solicitante solicitante, Profesion profesion)
    {
        var ahora = DateTime.UtcNow;
        return new Solicitud
        {
            Solicitante = solicitante,
            SolicitanteId = solicitante.Id,
            Profesion = profesion,
            ProfesionId = profesion.Id,
            TipoTramite = profesion.TipoTramite,
            FechaCreacion = ahora,
            FechaActualizacion = ahora
        };
    }

    public void Radicar(string usuarioId)
    {
        if (Estado != EstadoSolicitud.Borrador)
        {
            throw new DomainException("La solicitud ya fue radicada.");
        }

        if (DatosAcademicos is null)
        {
            throw new DomainException("Debe completar los datos academicos antes de radicar la solicitud.");
        }

        FechaRadicacion = DateTime.UtcNow;
        Estado = EstadoSolicitud.EnProceso;
        EtapaActual = EtapaSolicitud.Etapa1;
        FechaActualizacion = FechaRadicacion.Value;

        Historial.Add(new HistorialEstado
        {
            EstadoResultante = Estado,
            EtapaResultante = EtapaActual,
            UsuarioId = usuarioId,
            Motivo = "Solicitud radicada",
            Fecha = FechaRadicacion.Value
        });
    }

    public void Aprobar(string usuarioId)
    {
        AsegurarEnProceso();

        EtapaActual = EtapaActual switch
        {
            EtapaSolicitud.Etapa1 => EtapaSolicitud.Etapa2,
            EtapaSolicitud.Etapa2 => EtapaSolicitud.Etapa3,
            EtapaSolicitud.Etapa3 => EtapaSolicitud.Inventario,
            EtapaSolicitud.Inventario => AprobarDesdeInventario(),
            _ => throw new DomainException("Etapa de solicitud desconocida.")
        };

        FechaActualizacion = DateTime.UtcNow;
        Historial.Add(new HistorialEstado
        {
            EstadoResultante = Estado,
            EtapaResultante = EtapaActual,
            UsuarioId = usuarioId,
            Motivo = "Aprobada",
            Fecha = FechaActualizacion
        });
    }

    private EtapaSolicitud AprobarDesdeInventario()
    {
        if (Consecutivo is null)
        {
            throw new DomainException("No se puede aprobar en Inventario sin un consecutivo asignado.");
        }

        Estado = EstadoSolicitud.Aprobado;
        return EtapaSolicitud.Inventario;
    }

    public void Rechazar(string usuarioId, string motivo)
    {
        AsegurarEnProceso();
        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new DomainException("El motivo de rechazo es obligatorio.");
        }

        Estado = EstadoSolicitud.Rechazado;
        FechaActualizacion = DateTime.UtcNow;

        Historial.Add(new HistorialEstado
        {
            EstadoResultante = Estado,
            EtapaResultante = EtapaActual,
            UsuarioId = usuarioId,
            Motivo = motivo,
            Fecha = FechaActualizacion
        });
    }

    public void AsignarConsecutivo(string numero, ModoConsecutivo modo)
    {
        AsegurarEnProceso();
        if (EtapaActual != EtapaSolicitud.Inventario)
        {
            throw new DomainException("Solo se puede asignar consecutivo cuando la solicitud esta en Inventario.");
        }

        if (Consecutivo is not null)
        {
            throw new DomainException("Esta solicitud ya tiene un consecutivo asignado.");
        }

        if (string.IsNullOrWhiteSpace(numero))
        {
            throw new DomainException("El numero de consecutivo es obligatorio.");
        }

        Consecutivo = new Consecutivo
        {
            SolicitudId = Id,
            Numero = numero,
            Fecha = DateOnly.FromDateTime(DateTime.UtcNow),
            Modo = modo
        };
        FechaActualizacion = DateTime.UtcNow;
    }

    public void AdjuntarArchivo(ArchivoAdjunto archivo)
    {
        archivo.SolicitudId = Id;
        Archivos.Add(archivo);
    }

    public void AgregarComentario(Comentario comentario)
    {
        comentario.SolicitudId = Id;
        Comentarios.Add(comentario);
    }

    private void AsegurarEnProceso()
    {
        if (Estado != EstadoSolicitud.EnProceso)
        {
            throw new DomainException("La solicitud no esta en proceso de revision (esta en borrador, aprobada o rechazada).");
        }
    }
}
