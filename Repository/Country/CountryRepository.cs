using System.Text.Json;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext db)
            : base(db)
        {
            _context = db;
        }

        public void LoadCountriesJsonToBd()
        {
            var jsonFilePath = "./resources/loadFiles/countries.json";
            using var fileStream = new FileStream(jsonFilePath, FileMode.Open);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                WriteIndented = false
            };

            var countries = JsonSerializer.Deserialize<List<Country>>(fileStream, options);

            // Agregar los países al DbSet
            _context.Country.AddRange(countries);
            _context.SaveChanges();
        }
    }
}
