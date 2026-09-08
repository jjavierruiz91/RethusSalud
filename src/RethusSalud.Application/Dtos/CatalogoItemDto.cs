using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Dtos;

public record CatalogoItemDto(int Id, string Nombre);

public record ProfesionDto(int Id, string Nombre, TipoTramite TipoTramite, NivelFormacion NivelFormacion);
