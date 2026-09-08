using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Interfaces;

public interface ICatalogoRepository
{
    Task<List<Pais>> GetPaisesAsync();
    Task<List<Departamento>> GetDepartamentosAsync(int paisId);
    Task<List<Municipio>> GetMunicipiosAsync(int departamentoId);
    Task<List<Profesion>> GetProfesionesAsync(TipoTramite tipoTramite);
    Task<Profesion?> GetProfesionByIdAsync(int id);
}
