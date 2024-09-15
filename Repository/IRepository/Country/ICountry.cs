using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;

namespace rethus_backend.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        void LoadCountriesJsonToBd();

        public Task<List<CountryResponseDto>> GetCountries();
        public Task<CountryResponseDto> GetCountryId(int countryId);

        public Task<List<CountryResponseDto>> GetCountriesPagination(
            int pageNumber,
            int pageSize = 10
        );
    }
}
