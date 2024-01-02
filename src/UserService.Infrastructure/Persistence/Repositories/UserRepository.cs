using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Infrastructure.Persistence.Repositories
{
    internal sealed class UserRepository : Repository<User>, IUserRepository
    {
        // Add here any methods that you need for the repository, basic CRUD operations are already implemented in the generic repository

        // Access context via DbContext

        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
