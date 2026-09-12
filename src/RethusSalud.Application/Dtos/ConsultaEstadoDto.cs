using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Dtos;

public record ConsultaEstadoDto(
    string NombreCompleto,
    bool TieneSolicitud,
    EstadoSolicitud? Estado,
    EtapaSolicitud? EtapaActual);
