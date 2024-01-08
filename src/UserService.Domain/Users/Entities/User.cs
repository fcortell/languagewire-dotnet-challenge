using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Common;

namespace UserService.Domain.Users.Entities
{
    public class User : BaseEntity
    {
        public string? Name { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public int TranslationBalance { get; set; } = 0;
        public int TotalSpentAmount { get; set; } = 0;

    }
}
