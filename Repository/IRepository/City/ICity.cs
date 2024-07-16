using rethus_backend.Models;

namespace rethus_backend.Repository.IRepository
{
    public interface ICityRepository : IRepository<City>
    {
        void LoadCityJsonToBd();
        public Task<List<CityResponseDto>> GetCities(int departmentId);
    }
}
