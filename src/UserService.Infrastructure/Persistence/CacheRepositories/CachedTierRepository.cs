using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using UserService.Domain.Tiers;
using UserService.Domain.Tiers.Entities;
using UserService.Infrastructure.Persistence.Repositories;

namespace UserService.Infrastructure.Persistence.CacheRepositories
{
    // With this we added a wrapper class that just calls existing methods.
    // With that, we can inject additional behaviour, like a cache layer to the TierRepository, so that we can avoid unnecessary calls to the database and
    public class CachedTierRepository : ITierRepository
    {
        private readonly IMemoryCache _cache;
        private readonly TierRepository _decorated;

        public CachedTierRepository(TierRepository decorated, IMemoryCache cache)
        {
            _decorated = decorated;
            _cache = cache;
        }

        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            return _decorated.CommitChangesAsync(cancellationToken);
        }

        public void Delete(Tier entity)
        {
            _decorated.Delete(entity);
        }

        public Task<IEnumerable<Tier?>> Find(Expression<Func<Tier, bool>> predicate)
        {
            return _decorated.Find(predicate);
        }

        public Task<IEnumerable<Tier?>?> GetAllAsync()
        {
            string key = "TierRepository.GetAllAsync";
            return _cache.GetOrCreateAsync(
            key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromDays(30));
                return _decorated.GetAllAsync();
            });
        }

        public Task<Tier?> GetByIdAsync(long id)
        {
            return _decorated.GetByIdAsync(id);
        }

        public async Task<Tier> GetTierByRangeAsync(int userTotalSpentAmount)
        {
            List<Tier?>? tiers = (await GetAllAsync())?.ToList();
            if (tiers == null)
            {
                return new Tier();
            }
            Tier? result = tiers.Where(t => t.RangeStart <= userTotalSpentAmount && t.RangeEnd > userTotalSpentAmount).FirstOrDefault();
            if (result is null)
            {
                return new Tier()
                {
                    RangeStart = 0,
                    RangeEnd = 0,
                    Name = "No tier"
                };
            }
            return result;
        }

        public void Insert(Tier entity)
        {
            _decorated.Insert(entity);
        }

        public void Update(Tier entity)
        {
            _decorated.Update(entity);
        }
    }
}