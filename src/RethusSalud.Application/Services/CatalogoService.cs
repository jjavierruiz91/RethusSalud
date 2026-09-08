using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Services;

public class CatalogoService
{
    private readonly ICatalogoRepository _catalogos;

    public CatalogoService(ICatalogoRepository catalogos)
    {
        _catalogos = catalogos;
    }

    public async Task<List<CatalogoItemDto>> ObtenerPaisesAsync() =>
        (await _catalogos.GetPaisesAsync()).Select(p => new CatalogoItemDto(p.Id, p.Nombre)).ToList();

    public async Task<List<CatalogoItemDto>> ObtenerDepartamentosAsync(int paisId) =>
        (await _catalogos.GetDepartamentosAsync(paisId)).Select(d => new CatalogoItemDto(d.Id, d.Nombre)).ToList();

    public async Task<List<CatalogoItemDto>> ObtenerMunicipiosAsync(int departamentoId) =>
        (await _catalogos.GetMunicipiosAsync(departamentoId)).Select(m => new CatalogoItemDto(m.Id, m.Nombre)).ToList();

    public async Task<List<ProfesionDto>> ObtenerProfesionesAsync(TipoTramite tipoTramite) =>
        (await _catalogos.GetProfesionesAsync(tipoTramite))
            .Select(p => new ProfesionDto(p.Id, p.Nombre, p.TipoTramite, p.NivelFormacion))
            .ToList();
}
