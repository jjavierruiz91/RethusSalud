using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var jsonFilePath = "./resources/loadFiles/country-state-city/country.json";
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

        public Task<List<CountryResponseDto>> GetCountries()
        {
            var response = new ApiResponse();

            var countries = _context.Country
                .Select(c => new CountryResponseDto { Id = c.CountryId, Name = c.Name })
                .ToListAsync();

            return countries;
        }

        public async Task<CountryResponseDto> GetCountryId(int countryId)
        {
            var country = _context.Country
                .Where(c => c.CountryId == countryId)
                .Select(c => new CountryResponseDto { Id = c.CountryId, Name = c.Name })
                .FirstOrDefault();

            return country;
        }
    }
}
