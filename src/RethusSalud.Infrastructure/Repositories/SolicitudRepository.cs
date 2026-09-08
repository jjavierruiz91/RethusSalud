using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
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
            .Include(s => s.Solicitante)
            .Include(s => s.Profesion)
            .Include(s => s.DatosAcademicos)
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
