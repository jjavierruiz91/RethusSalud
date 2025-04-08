using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;
using rethus_backend.Repository.IRepository.Cache;

namespace rethus_backend.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly MemoryCacheRepository<Country> _memoryCache;

        public CountryRepository(ApplicationDbContext db, IMemoryCache memoryCache)
            : base(db)
        {
            _context = db;
            _memoryCache = new MemoryCacheRepository<Country>(memoryCache);
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
                .AsNoTracking()
                .Select(c => new CountryResponseDto { Id = c.CountryId, Name = c.Name })
                .OrderByDescending(c => c.Name)
                .ToListAsync();
            return countries;
        }

        public async Task<List<CountryResponseDto>> GetCountriesPagination(
            int pageNumber,
            int pageSize = 10
        )
        {
            var cacheKey = $"COUNTRIES_PAGE_{pageNumber}_SIZE_{pageSize}";
            List<Country> countries;

            countries = _memoryCache.GetListFromCache(cacheKey);
            if (countries == null)
            {
                countries = await _context.Country
                    .AsNoTracking()
                    .OrderBy(c => c.CountryId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(c => c.Name)
                    .ToListAsync();

                var cacheDuration = TimeSpan.FromDays(7);
                _memoryCache.SetList(cacheKey, countries, cacheDuration);
            }
            var response = countries.Select(c => MapToDto(c)).ToList();

            return response;
        }

        public async Task<CountryResponseDto> GetCountryId(int countryId)
        {
            var cacheKey = $"COUNTRY_{countryId}"; // Clave para el caché
            Country country;

            country = _memoryCache.GetFromCache(cacheKey);

            if (country == null)
            {
                country = await _context.Country
                    .Where(c => c.CountryId == countryId)
                    .FirstOrDefaultAsync();

                if (country != null)
                {
                    var cacheDuration = TimeSpan.FromDays(7);
                    _memoryCache.SetToCache(cacheKey, country, cacheDuration);
                }
            }
            return MapToDto(country);
        }

        private CountryResponseDto MapToDto(Country country)
        {
            return new CountryResponseDto { Id = country.CountryId, Name = country.Name };
        }
    }
}
