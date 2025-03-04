using Microsoft.EntityFrameworkCore;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
    public class UserDigitalSignatureRepository
        : Repository<UserDigitalSignature>,
            IUserDigitalSignatureRepository
    {
        private readonly ApplicationDbContext _context;

        public UserDigitalSignatureRepository(ApplicationDbContext db)
            : base(db)
        {
            _context = db;
        }

        public async Task<UserDigitalSignature?> GetByUserIdAsync(string userId)
        {
            return await _context.UserDigitalSignature.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<IEnumerable<UserDigitalSignature>> GetAllActiveAsync()
        {
            return await _context.UserDigitalSignature
                .Where(s => s.Status == SignatureStatus.Active)
                .ToListAsync();
        }

        public async Task AddAsync(UserDigitalSignature signature)
        {
            await _context.UserDigitalSignature.AddAsync(signature);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserDigitalSignature signature)
        {
            _context.UserDigitalSignature.Update(signature);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId)
        {
            var signature = await GetByUserIdAsync(userId);
            if (signature != null)
            {
                _context.UserDigitalSignature.Remove(signature);
                await _context.SaveChangesAsync();
            }
        }

        public Task<bool> IsSignatureInUseAsync(SignatureType signatureType, SignatureStatus status)
        {
            return _context.UserDigitalSignature.AnyAsync(
                s => s.SignatureType == signatureType && s.Status == status
            );
        }

        public Task<UserDigitalSignature?> GetByIdAsync(string Id)
        {
            return _context.UserDigitalSignature.FirstOrDefaultAsync(
                s => s.UserDigitalSignatureId == Id
            );
        }

        public async Task<bool> IsSignatureInUseByUserIdAsync(
            string userId,
            string userDigitalSignatureId
        )
        {
            return await _context.UserDigitalSignature.AnyAsync(
                s => s.UserId == userId && s.UserDigitalSignatureId == userDigitalSignatureId
            );
        }

        public async Task<bool> IsUserSignatureActiveForTypesAsync()
        {
            // Los tipos de firma que deben estar activos
            var requiredSignatureTypes = new[]
            {
                SignatureType.Secretary,
                SignatureType.Approve,
                SignatureType.Review,
                SignatureType.Project
            };

            // Verificamos si todos los tipos de firma requeridos están activos
            var activeSignatureTypes = await _context.UserDigitalSignature
                .Where(s => s.Status == SignatureStatus.Active)
                .Select(s => s.SignatureType)
                .ToListAsync();

            // Comprobamos si todos los tipos requeridos están en la lista de tipos activos
            return requiredSignatureTypes.All(type => activeSignatureTypes.Contains(type));
        }

        public async Task<bool> IsUserSignatureActiveAsync(string userId, SignatureStatus status)
        {
            return await _context.UserDigitalSignature.AnyAsync(
                s => s.UserId == userId && s.Status == status
            );
        }
    }
}
