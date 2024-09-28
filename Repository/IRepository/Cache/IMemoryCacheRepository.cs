using rethus_backend.Models;

namespace rethus_backend.Repository.IRepository.Cache
{
    public interface IMemoryCacheRepository<T>
        where T : class
    {
        T GetFromCache(string cacheKey);
        void SetToCache(string cacheKey, T item, TimeSpan cacheDuration);
        void RemoveFromCache(string cacheKey);
        List<T> GetListFromCache(string cacheKey);
        public void SetList(string cacheKey, List<T> items, TimeSpan cacheDuration);
    }
}
