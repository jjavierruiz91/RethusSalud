using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Application.Services;

public class ConfiguracionInstitucionalService
{
    private readonly IConfiguracionInstitucionalRepository _repositorio;

    public ConfiguracionInstitucionalService(IConfiguracionInstitucionalRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<ConfiguracionInstitucional?> ObtenerAsync() => _repositorio.ObtenerAsync();

    public Task GuardarAsync(string nombre, string cargo, string? firmaUrl) =>
        _repositorio.GuardarAsync(new ConfiguracionInstitucional
        {
            Nombre = nombre,
            Cargo = cargo,
            FirmaUrl = firmaUrl
        });
}
