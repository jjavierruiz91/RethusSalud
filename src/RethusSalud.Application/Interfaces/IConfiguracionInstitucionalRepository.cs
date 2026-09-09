using RethusSalud.Domain.Entities;

namespace RethusSalud.Application.Interfaces;

public interface IConfiguracionInstitucionalRepository
{
    Task<ConfiguracionInstitucional?> ObtenerAsync();
    Task GuardarAsync(ConfiguracionInstitucional configuracion);
}
