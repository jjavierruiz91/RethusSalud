using Microsoft.EntityFrameworkCore;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Infrastructure.Persistence;

namespace RethusSalud.Infrastructure.Repositories;

public class ConfiguracionInstitucionalRepository : IConfiguracionInstitucionalRepository
{
    private readonly ApplicationDbContext _context;

    public ConfiguracionInstitucionalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ConfiguracionInstitucional?> ObtenerAsync() =>
        _context.ConfiguracionesInstitucionales.FirstOrDefaultAsync();

    public async Task GuardarAsync(ConfiguracionInstitucional configuracion)
    {
        var existente = await _context.ConfiguracionesInstitucionales.FirstOrDefaultAsync();
        if (existente is null)
        {
            _context.ConfiguracionesInstitucionales.Add(configuracion);
        }
        else
        {
            existente.Nombre = configuracion.Nombre;
            existente.Cargo = configuracion.Cargo;
            if (!string.IsNullOrEmpty(configuracion.FirmaUrl))
            {
                existente.FirmaUrl = configuracion.FirmaUrl;
            }
        }

        await _context.SaveChangesAsync();
    }
}
