using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository.Cache;

namespace rethus_backend.Repository
{
    public class MemoryCacheRepository<T> : IMemoryCacheRepository<T>
        where T : class
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetFromCache(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out T item);
            return item;
        }

        public Task RemoveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public void SetToCache(string cacheKey, T item, TimeSpan cacheDuration)
        {
            _memoryCache.Set(cacheKey, item, cacheDuration);
        }

        public List<T> GetListFromCache(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out List<T> item);
            return item;
        }

        public void SetList(string cacheKey, List<T> items, TimeSpan cacheDuration)
        {
            _memoryCache.Set(cacheKey, items, cacheDuration);
        }
    }
}
