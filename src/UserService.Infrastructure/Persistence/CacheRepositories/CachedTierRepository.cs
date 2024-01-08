using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
        private readonly ITierRepository _decorated;
        private readonly IMemoryCache _cache;

        public CachedTierRepository(ITierRepository decorated, IMemoryCache cache)
        {
            _decorated = decorated;
            _cache = cache;
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

        public void Insert(Tier entity)
        {
            _decorated.Insert(entity);
        }

        public void Update(Tier entity)
        {
            _decorated.Update(entity);
        }

        public void Delete(Tier entity)
        {
            _decorated.Delete(entity);
        }

        public Task<Tier?> GetByIdAsync(long id)
        {
            return _decorated.GetByIdAsync(id);
        }

        public Task<IEnumerable<Tier?>> Find(Expression<Func<Tier, bool>> predicate)
        {
            return _decorated.Find(predicate);
        }

        public Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            return _decorated.CommitChangesAsync(cancellationToken);
        }

    }
}
