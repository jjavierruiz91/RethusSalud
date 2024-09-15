using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly ApplicationDbContext _context;

        public CityRepository(ApplicationDbContext db)
            : base(db)
        {
            _context = db;
        }

        public Task<List<CityResponseDto>> GetCities(int departmentId)
        {
            var response = new ApiResponse();

            var cities = _context.City
                .AsNoTracking()
                .Where(c => c.DepartmentId == departmentId)
                .Select(c => new CityResponseDto { Id = c.CityId, Name = c.Name })
                .ToListAsync();

            return cities;
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
    }
}
