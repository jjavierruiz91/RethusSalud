using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
using PagedResult = RethusSalud.Application.Common.PagedResult<RethusSalud.Domain.Entities.Solicitud>;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Persistence;

namespace RethusSalud.Infrastructure.Repositories;

public class SolicitudRepository : ISolicitudRepository
{
    private readonly ApplicationDbContext _context;

    public SolicitudRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private IQueryable<Solicitud> ConCargasCompletas() =>
        _context.Solicitudes
            .Include(s => s.Solicitante).ThenInclude(s => s.PaisNacimiento)
            .Include(s => s.Solicitante).ThenInclude(s => s.DepartamentoNacimiento)
            .Include(s => s.Solicitante).ThenInclude(s => s.MunicipioNacimiento)
            .Include(s => s.Solicitante).ThenInclude(s => s.PaisResidencia)
            .Include(s => s.Solicitante).ThenInclude(s => s.DepartamentoResidencia)
            .Include(s => s.Solicitante).ThenInclude(s => s.MunicipioResidencia)
            .Include(s => s.Profesion)
            .Include(s => s.DatosAcademicos).ThenInclude(d => d!.PaisInstitucion)
            .Include(s => s.DatosAcademicos).ThenInclude(d => d!.DepartamentoInstitucion)
            .Include(s => s.DatosAcademicos).ThenInclude(d => d!.MunicipioInstitucion)
            .Include(s => s.Consecutivo)
            .Include(s => s.Archivos)
            .Include(s => s.Comentarios)
            .Include(s => s.Historial);

    public async Task AddAsync(Solicitud solicitud)
    {
        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();
    }

    public Task<Solicitud?> GetByIdAsync(int id) =>
        ConCargasCompletas().FirstOrDefaultAsync(s => s.Id == id);

    public Task<Solicitud?> GetBorradorActivoAsync(int solicitanteId) =>
        ConCargasCompletas()
            .Where(s => s.SolicitanteId == solicitanteId && s.Estado == EstadoSolicitud.Borrador)
            .OrderByDescending(s => s.FechaCreacion)
            .FirstOrDefaultAsync();

    public Task<Solicitud?> GetUltimaBySolicitanteIdAsync(int solicitanteId) =>
        ConCargasCompletas()
            .Where(s => s.SolicitanteId == solicitanteId)
            .OrderByDescending(s => s.FechaCreacion)
            .FirstOrDefaultAsync();

    public async Task<List<Solicitud>> GetPorEtapaAsync(EtapaSolicitud etapa, BandejaFiltroDto filtro)
    {
        var query = ConCargasCompletas().Where(s => s.EtapaActual == etapa);

        query = filtro.Estado.HasValue
            ? query.Where(s => s.Estado == filtro.Estado.Value)
            : query.Where(s => s.Estado == EstadoSolicitud.EnProceso);

        if (!string.IsNullOrWhiteSpace(filtro.NumeroIdentificacion))
        {
            query = query.Where(s => s.Solicitante.NumeroIdentificacion.Contains(filtro.NumeroIdentificacion));
        }

        if (filtro.TipoTramite.HasValue)
        {
            query = query.Where(s => s.TipoTramite == filtro.TipoTramite.Value);
        }

        if (filtro.Desde.HasValue)
        {
            query = query.Where(s => s.FechaCreacion >= filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(s => s.FechaCreacion <= filtro.Hasta.Value);
        }

        return await query.OrderByDescending(s => s.FechaCreacion).ToListAsync();
    }

    public async Task<List<Solicitud>> GetTodasAsync(BandejaFiltroDto filtro)
    {
        var query = ConCargasCompletas().Where(s => s.Estado != EstadoSolicitud.Borrador);

        if (filtro.Estado.HasValue)
        {
            query = query.Where(s => s.Estado == filtro.Estado.Value);
        }

        if (filtro.Etapa.HasValue)
        {
            query = query.Where(s => s.EtapaActual == filtro.Etapa.Value);
        }

        if (!string.IsNullOrWhiteSpace(filtro.NumeroIdentificacion))
        {
            query = query.Where(s => s.Solicitante.NumeroIdentificacion.Contains(filtro.NumeroIdentificacion));
        }

        if (filtro.TipoTramite.HasValue)
        {
            query = query.Where(s => s.TipoTramite == filtro.TipoTramite.Value);
        }

        if (filtro.Desde.HasValue)
        {
            query = query.Where(s => s.FechaCreacion >= filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(s => s.FechaCreacion <= filtro.Hasta.Value);
        }

        return await query.OrderByDescending(s => s.FechaCreacion).ToListAsync();
    }

    private IQueryable<Solicitud> ParaListado() =>
        _context.Solicitudes
            .Include(s => s.Solicitante)
            .Include(s => s.Consecutivo);

    private static IQueryable<Solicitud> AplicarFiltrosComunes(IQueryable<Solicitud> query, BandejaFiltroDto filtro)
    {
        if (!string.IsNullOrWhiteSpace(filtro.NumeroIdentificacion))
        {
            query = query.Where(s => s.Solicitante.NumeroIdentificacion.Contains(filtro.NumeroIdentificacion));
        }

        if (filtro.TipoTramite.HasValue)
        {
            query = query.Where(s => s.TipoTramite == filtro.TipoTramite.Value);
        }

        if (filtro.Desde.HasValue)
        {
            query = query.Where(s => s.FechaCreacion >= filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(s => s.FechaCreacion <= filtro.Hasta.Value);
        }

        return query;
    }

    public async Task<BandejaPagedResult> GetPorEtapaPagedAsync(EtapaSolicitud etapa, BandejaFiltroDto filtro, int pageNumber, int pageSize)
    {
        var query = ParaListado().Where(s => s.EtapaActual == etapa);
        query = filtro.Estado.HasValue
            ? query.Where(s => s.Estado == filtro.Estado.Value)
            : query.Where(s => s.Estado == EstadoSolicitud.EnProceso);
        query = AplicarFiltrosComunes(query, filtro);

        var totalCount = await query.CountAsync();
        var totalEnProceso = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso);
        var totalAprobadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Aprobado);
        var limiteEspera = DateTime.UtcNow.AddDays(-2);
        var totalEsperandoLargo = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso && s.FechaCreacion <= limiteEspera);

        var items = await query
            .OrderByDescending(s => s.FechaCreacion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new BandejaPagedResult
        {
            Pagina = new PagedResult { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount },
            TotalEnProceso = totalEnProceso,
            TotalAprobadas = totalAprobadas,
            TotalEsperandoLargo = totalEsperandoLargo
        };
    }

    private IQueryable<Solicitud> ConstruirQueryTodas(BandejaFiltroDto filtro)
    {
        var query = ParaListado().Where(s => s.Estado != EstadoSolicitud.Borrador);

        if (filtro.Estado.HasValue)
        {
            query = query.Where(s => s.Estado == filtro.Estado.Value);
        }

        if (filtro.Etapa.HasValue)
        {
            query = query.Where(s => s.EtapaActual == filtro.Etapa.Value);
        }

        return AplicarFiltrosComunes(query, filtro);
    }

    public async Task<PagedResult> GetTodasPagedAsync(BandejaFiltroDto filtro, int pageNumber, int pageSize)
    {
        var query = ConstruirQueryTodas(filtro);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.FechaCreacion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount };
    }

    public async Task<SeguimientoPagedResult> GetSeguimientoPagedAsync(BandejaFiltroDto filtro, int pageNumber, int pageSize)
    {
        var query = ConstruirQueryTodas(filtro);

        var totalCount = await query.CountAsync();
        var totalEnProceso = await query.CountAsync(s => s.Estado == EstadoSolicitud.EnProceso);
        var totalAprobadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Aprobado);
        var totalRechazadas = await query.CountAsync(s => s.Estado == EstadoSolicitud.Rechazado);

        var items = await query
            .OrderByDescending(s => s.FechaCreacion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new SeguimientoPagedResult
        {
            Pagina = new PagedResult { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount },
            TotalEnProceso = totalEnProceso,
            TotalAprobadas = totalAprobadas,
            TotalRechazadas = totalRechazadas
        };
    }

    public Task<ArchivoAdjunto?> GetArchivoByIdAsync(int archivoId) =>
        _context.ArchivosAdjuntos.FirstOrDefaultAsync(a => a.Id == archivoId);

    public Task<Solicitud?> GetByConsecutivoAsync(string numero) =>
        ConCargasCompletas().FirstOrDefaultAsync(s => s.Consecutivo != null && s.Consecutivo.Numero == numero);

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (EsViolacionDeUnicidad(ex))
        {
            throw new AppValidationException(new[] { "Ese numero de consecutivo ya existe. Elige otro." });
        }
    }

    private static bool EsViolacionDeUnicidad(DbUpdateException ex) =>
        ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627);
}
