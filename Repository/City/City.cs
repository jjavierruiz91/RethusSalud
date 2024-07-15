using System.Text.Json;
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

        public void LoadCityJsonToBd()
        {
            var jsonFilePath = "./resources/loadFiles/city.json";
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
