using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Tiers;
using UserService.Domain.Tiers.Entities;

namespace UserService.Infrastructure.Persistence.Repositories
{
    public sealed class TierRepository : GenericRepository<Tier>, ITierRepository
    {
        public TierRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
