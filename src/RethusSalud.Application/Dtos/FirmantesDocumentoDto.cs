namespace RethusSalud.Application.Dtos;

public record FirmantesDocumentoDto(
    FirmanteDto? Proyecto,
    FirmanteDto? Aprobo,
    FirmanteDto? Reviso,
    FirmanteDto? Genero,
    FirmanteDto? Jefe);
