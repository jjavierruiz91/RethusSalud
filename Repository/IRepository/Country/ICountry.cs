using rethus_backend.Models;

namespace rethus_backend.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        void LoadCountriesJsonToBd();
    }
}
