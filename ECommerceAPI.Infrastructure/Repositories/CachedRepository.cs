using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using ECommerceAPI.Domain.Common;
using ECommerceAPI.Domain.Interfaces;

namespace ECommerceAPI.Infrastructure.Repositories
{
    // The Decorator Pattern wrapping IRepository
    public class CachedRepository<T> : IRepository<T> where T : Entity
    {
        private readonly IRepository<T> _innerRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        // Expects the concreted generic repository injected here
        public CachedRepository(IRepository<T> innerRepository, IMemoryCache memoryCache)
        {
            _innerRepository = innerRepository;
            _memoryCache = memoryCache;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{typeof(T).Name}_{id}";
            
            if (!_memoryCache.TryGetValue(cacheKey, out T? cachedEntity))
            {
                cachedEntity = await _innerRepository.GetByIdAsync(id);
                if (cachedEntity != null)
                {
                    _memoryCache.Set(cacheKey, cachedEntity, _cacheDuration);
                }
            }

            return cachedEntity;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            // Simple approach: list not cached for demonstration
            return await _innerRepository.GetAllAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _innerRepository.AddAsync(entity);
            // Optionally clear relevant caches
        }

        public void Update(T entity)
        {
            _innerRepository.Update(entity);
            _memoryCache.Remove($"{typeof(T).Name}_{entity.Id}");
        }

        public void Delete(T entity)
        {
            _innerRepository.Delete(entity);
            _memoryCache.Remove($"{typeof(T).Name}_{entity.Id}");
        }
    }
}
