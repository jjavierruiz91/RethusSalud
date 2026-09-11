using Microsoft.EntityFrameworkCore;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Persistence;

namespace RethusSalud.Infrastructure.Repositories;

public class CatalogoRepository : ICatalogoRepository
{
    private readonly ApplicationDbContext _context;

    public CatalogoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Pais>> GetPaisesAsync() =>
        _context.Paises.AsNoTracking()
            .OrderBy(p => p.Nombre == "Colombia" ? 0 : 1)
            .ThenBy(p => p.Nombre)
            .ToListAsync();

    public Task<List<Departamento>> GetDepartamentosAsync(int paisId) =>
        _context.Departamentos.AsNoTracking()
            .Where(d => d.PaisId == paisId)
            .OrderBy(d => d.Nombre)
            .ToListAsync();

    public Task<List<Departamento>> GetTodosDepartamentosAsync() =>
        _context.Departamentos.AsNoTracking()
            .OrderBy(d => d.Nombre)
            .ToListAsync();

    public Task<List<Municipio>> GetMunicipiosAsync(int departamentoId) =>
        _context.Municipios.AsNoTracking()
            .Where(m => m.DepartamentoId == departamentoId)
            .OrderBy(m => m.Nombre)
            .ToListAsync();

    public Task<List<Profesion>> GetProfesionesAsync(TipoTramite tipoTramite) =>
        _context.Profesiones.AsNoTracking()
            .Where(p => p.TipoTramite == tipoTramite)
            .OrderBy(p => p.NivelFormacion).ThenBy(p => p.Nombre)
            .ToListAsync();

    public Task<Profesion?> GetProfesionByIdAsync(int id) =>
        _context.Profesiones.FirstOrDefaultAsync(p => p.Id == id);
}
