using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Dtos;

public record BandejaFiltroDto(
    string? NumeroIdentificacion = null,
    TipoTramite? TipoTramite = null,
    EstadoSolicitud? Estado = null,
    DateTime? Desde = null,
    DateTime? Hasta = null,
    EtapaSolicitud? Etapa = null);
