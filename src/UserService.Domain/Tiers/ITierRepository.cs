using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Tiers.Entities;
using UserService.Domain.Users.Entities;

namespace UserService.Domain.Tiers
{
    public interface ITierRepository : IGenericRepository<Tier>
    {
    }
}
