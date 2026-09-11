using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Interfaces;

public interface ISolicitudRepository
{
    Task AddAsync(Solicitud solicitud);
    Task<Solicitud?> GetByIdAsync(int id);
    Task<Solicitud?> GetBorradorActivoAsync(int solicitanteId);
    Task<Solicitud?> GetUltimaBySolicitanteIdAsync(int solicitanteId);
    Task<List<Solicitud>> GetPorEtapaAsync(EtapaSolicitud etapa, BandejaFiltroDto filtro);
    Task<List<Solicitud>> GetTodasAsync(BandejaFiltroDto filtro);
    Task<BandejaPagedResult> GetPorEtapaPagedAsync(EtapaSolicitud etapa, BandejaFiltroDto filtro, int pageNumber, int pageSize);
    Task<PagedResult<Solicitud>> GetTodasPagedAsync(BandejaFiltroDto filtro, int pageNumber, int pageSize);
    Task<SeguimientoPagedResult> GetSeguimientoPagedAsync(BandejaFiltroDto filtro, int pageNumber, int pageSize);
    Task<DashboardResultDto> GetDashboardAsync(DashboardFiltroDto filtro);
    Task<Solicitud?> GetByConsecutivoAsync(string numero);
    Task<ArchivoAdjunto?> GetArchivoByIdAsync(int archivoId);
    Task SaveChangesAsync();
}
