using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Repositories.Persons
{
    public abstract class BasePersonRepository<TEntity>(ApplicationDbContext context, IMemoryCache cache) : IPersonRepository<TEntity> where TEntity : class, IEntity
    {
        private static readonly object cacheLock = new();
        private static readonly ConcurrentDictionary<string, bool> cacheKeys = [];

        protected readonly ApplicationDbContext context = context ?? throw new ArgumentNullException(nameof(context));
        protected readonly IMemoryCache cache = cache ?? throw new ArgumentNullException(nameof(cache));
        protected abstract string CacheKeyPrefix { get; }


        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(int pageNumber, int pageSize, string sortBy, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cacheKey = GetCacheKey(pageNumber, pageSize, sortBy);

            if (cache.TryGetValue(cacheKey, out IReadOnlyCollection<TEntity>? cachedEntities))
                return cachedEntities!;

            IQueryable<TEntity> query = ApplyIncludes(context.Set<TEntity>());

            query = ApplySorting(query, sortBy);

            var entities = await query
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            cache.Set(cacheKey, entities, cacheEntryOptions);

            cacheKeys.TryAdd(cacheKey, true);

            return entities;
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Set<TEntity>().FindAsync([id], cancellationToken);
        }

        public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            ClearCache();

            return entity.Id;
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);

            ClearCache();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await context.Set<TEntity>().FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            ClearCache();
        }

        protected abstract IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query);
        protected abstract IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string sortBy);

        protected virtual string GetCacheKey(int pageNumber, int pageSize, string sortBy)
        {
            return $"{CacheKeyPrefix}_{pageNumber}_{pageSize}_{sortBy}";
        }

        private void ClearCache()
        {
            lock (cacheLock)
            {
                var keysToRemove = cacheKeys.Keys.ToList();

                foreach (var key in keysToRemove)
                {
                    cache.Remove(key);
                    cacheKeys.TryRemove(key, out _);
                }
            }
        }
    }
}
