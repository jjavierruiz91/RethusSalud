using RethusSalud.Domain.Entities;

namespace RethusSalud.Application.Interfaces;

public interface ISolicitanteRepository
{
    Task<Solicitante?> GetByApplicationUserIdAsync(string applicationUserId);
    Task<Solicitante?> GetByNumeroIdentificacionAsync(string numeroIdentificacion);
    Task<bool> ExisteNumeroIdentificacionAsync(string numeroIdentificacion, int? excluirSolicitanteId = null);
    Task AddAsync(Solicitante solicitante);
    Task UpdateAsync(Solicitante solicitante);
}
