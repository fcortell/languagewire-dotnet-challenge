using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Tiers;
using UserService.Domain.Tiers.Entities;

namespace UserService.Infrastructure.Persistence.Repositories
{
    public sealed class TierRepository : GenericRepository<Tier>, ITierRepository
    {
        public TierRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Tier> GetTierByRangeAsync(int totalSpentAmount)
        {
            Tier tier = await Entities.Where(t => t.RangeStart <= totalSpentAmount && t.RangeEnd > totalSpentAmount).FirstOrDefaultAsync();
            if (tier is null)
            {
                tier = new Tier()
                {
                    RangeStart = 0,
                    RangeEnd = 0,
                    Name = "No tier"
                };
            }
            return tier;
        }
    }
}
