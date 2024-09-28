using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly MemoryCacheRepository<Department> _memoryCache;

        public DepartmentRepository(ApplicationDbContext db, IMemoryCache memoryCache)
            : base(db)
        {
            _context = db;
            _memoryCache = new MemoryCacheRepository<Department>(memoryCache);
        }

        public async Task<List<DepartmentResponseDto>> GetDepartments(int countryId)
        {
            var cacheKey = $"DEPARTMENTS_COUNTRY_{countryId}";
            List<Department> departments;

            departments = _memoryCache.GetListFromCache(cacheKey);

            if (departments == null)
            {
                departments = await _context.Departments
                    .AsNoTracking()
                    .Where(d => d.CountryId == countryId)
                    .ToListAsync();

                var cacheDuration = TimeSpan.FromDays(7);
                _memoryCache.SetList(cacheKey, departments, cacheDuration);
            }

            var response = departments.Select(d => MapToDepartmentDto(d)).ToList();
            return response;
        }

        public void LoadDeparmentJsonToBd()
        {
            var jsonFilePath = "./resources/loadFiles/country-state-city/deparment.json";
            using var fileStream = new FileStream(jsonFilePath, FileMode.Open);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                WriteIndented = false
            };

            var Department = JsonSerializer.Deserialize<List<Department>>(fileStream, options);

            _context.Departments.AddRange(Department);
            _context.SaveChanges();
        }

        private DepartmentResponseDto MapToDepartmentDto(Department department)
        {
            return new DepartmentResponseDto
            {
                Id = department.DepartmentId,
                Name = department.Name
            };
        }
    }
}
