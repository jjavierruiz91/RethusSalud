using RethusSalud.Application.Common;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Application.Dtos;

public class SeguimientoPagedResult
{
    public PagedResult<Solicitud> Pagina { get; init; } = new();
    public int TotalEnProceso { get; init; }
    public int TotalAprobadas { get; init; }
    public int TotalRechazadas { get; init; }
}
