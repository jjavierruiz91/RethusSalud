using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly MemoryCacheRepository<City> _memoryCache;

        public CityRepository(ApplicationDbContext db, IMemoryCache memoryCache)
            : base(db)
        {
            _context = db;
            _memoryCache = new MemoryCacheRepository<City>(memoryCache);
        }

        public async Task<List<CityResponseDto>> GetCities(int departmentId)
        {
            var cacheKey = $"CITIES_DEPARTMENT_{departmentId}";
            List<City> cities;

            // Intentar obtener la lista de ciudades desde la caché
            cities = _memoryCache.GetListFromCache(cacheKey);

            if (cities == null)
            {
                // Si no está en caché, consulta la base de datos
                cities = await _context.City
                    .AsNoTracking()
                    .Where(c => c.DepartmentId == departmentId)
                    .ToListAsync();

                // Establecer la lista de ciudades en caché por un tiempo específico
                var cacheDuration = TimeSpan.FromDays(7);
                _memoryCache.SetList(cacheKey, cities, cacheDuration);
            }

            // Aplicar el mapeo a CityResponseDto
            var response = cities.Select(c => MapToCityDto(c)).ToList();

            return response;
        }

        public void LoadCityJsonToBd()
        {
            var jsonFilePath = "./resources/loadFiles/country-state-city/city.json";
            using var fileStream = new FileStream(jsonFilePath, FileMode.Open);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                WriteIndented = false
            };

            var City = JsonSerializer.Deserialize<List<City>>(fileStream, options);

            // Agregar los países al DbSet
            _context.City.AddRange(City);
            _context.SaveChanges();
        }

        private CityResponseDto MapToCityDto(City city)
        {
            return new CityResponseDto { Id = city.CityId, Name = city.Name };
        }
    }
}
