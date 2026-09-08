using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Services;

public class ConsultaPublicaService
{
    private readonly ISolicitanteRepository _solicitantes;
    private readonly ISolicitudRepository _solicitudes;

    public ConsultaPublicaService(ISolicitanteRepository solicitantes, ISolicitudRepository solicitudes)
    {
        _solicitantes = solicitantes;
        _solicitudes = solicitudes;
    }

    public async Task<ConsultaEstadoDto?> ConsultarAsync(string numeroIdentificacion)
    {
        var solicitante = await _solicitantes.GetByNumeroIdentificacionAsync(numeroIdentificacion);
        if (solicitante is null)
        {
            return null;
        }

        var nombreCompleto = $"{solicitante.Nombres} {solicitante.Apellidos}".Trim();
        var solicitud = await _solicitudes.GetUltimaBySolicitanteIdAsync(solicitante.Id);

        if (solicitud is null)
        {
            return new ConsultaEstadoDto(nombreCompleto, "Sin solicitud registrada");
        }

        var estadoTexto = solicitud.Estado switch
        {
            EstadoSolicitud.Borrador => "En elaboracion (aun no ha sido enviada a revision)",
            EstadoSolicitud.EnProceso => $"En proceso - {solicitud.EtapaActual}",
            EstadoSolicitud.Aprobado => "Aprobado",
            EstadoSolicitud.Rechazado => "Rechazado",
            _ => "Desconocido"
        };

        return new ConsultaEstadoDto(nombreCompleto, estadoTexto);
    }

    public async Task<VerificacionFolioDto?> VerificarFolioAsync(string numero)
    {
        var solicitud = await _solicitudes.GetByConsecutivoAsync(numero);
        if (solicitud is null || solicitud.Estado != EstadoSolicitud.Aprobado || solicitud.Consecutivo is null)
        {
            return null;
        }

        return new VerificacionFolioDto(
            solicitud.Consecutivo.Numero,
            $"{solicitud.Solicitante.Nombres} {solicitud.Solicitante.Apellidos}".Trim(),
            solicitud.Profesion.Nombre,
            solicitud.TipoTramite.ToString(),
            solicitud.FechaActualizacion);
    }
}
