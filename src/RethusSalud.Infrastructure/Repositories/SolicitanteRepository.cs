using Microsoft.EntityFrameworkCore;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Infrastructure.Persistence;

namespace RethusSalud.Infrastructure.Repositories;

public class SolicitanteRepository : ISolicitanteRepository
{
    private readonly ApplicationDbContext _context;

    public SolicitanteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Solicitante?> GetByApplicationUserIdAsync(string applicationUserId) =>
        _context.Solicitantes.FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId);

    public Task<Solicitante?> GetByNumeroIdentificacionAsync(string numeroIdentificacion) =>
        _context.Solicitantes.FirstOrDefaultAsync(s => s.NumeroIdentificacion == numeroIdentificacion);

    public Task<bool> ExisteNumeroIdentificacionAsync(string numeroIdentificacion, int? excluirSolicitanteId = null) =>
        _context.Solicitantes.AnyAsync(s =>
            s.NumeroIdentificacion == numeroIdentificacion &&
            (excluirSolicitanteId == null || s.Id != excluirSolicitanteId));

    public async Task AddAsync(Solicitante solicitante)
    {
        _context.Solicitantes.Add(solicitante);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Solicitante solicitante)
    {
        _context.Solicitantes.Update(solicitante);
        await _context.SaveChangesAsync();
    }
}
