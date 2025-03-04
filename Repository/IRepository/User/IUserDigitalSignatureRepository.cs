using rethus_backend.Models;

namespace rethus_backend.Repository.IRepository
{
    public interface IUserDigitalSignatureRepository : IRepository<UserDigitalSignature>
    {
        Task<UserDigitalSignature?> GetByUserIdAsync(string userId);
        Task<UserDigitalSignature?> GetByIdAsync(string Id);
        Task<IEnumerable<UserDigitalSignature>> GetAllActiveAsync();
        Task AddAsync(UserDigitalSignature signature);
        Task UpdateAsync(UserDigitalSignature signature);
        Task DeleteAsync(string userId);
        Task<bool> IsSignatureInUseAsync(SignatureType signatureType, SignatureStatus status);
        Task<bool> IsSignatureInUseByUserIdAsync(string userId, string userDigitalSignatureId);
        Task<bool> IsUserSignatureActiveForTypesAsync();
        Task<bool> IsUserSignatureActiveAsync(string userId, SignatureStatus status);
    }
}
