using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Common;

namespace UserService.Domain.Tiers.Entities
{
    public class Tier : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int RangeStart { get; set; } = 0;
        public int RangeEnd { get; set; } = 0;

    }
}
