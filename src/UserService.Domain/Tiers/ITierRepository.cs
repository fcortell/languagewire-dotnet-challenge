using UserService.Domain.Tiers.Entities;

namespace UserService.Domain.Tiers
{
    public interface ITierRepository : IGenericRepository<Tier>
    {
        Task<Tier> GetTierByRangeAsync(int userTotalSpentAmount);
    }
}