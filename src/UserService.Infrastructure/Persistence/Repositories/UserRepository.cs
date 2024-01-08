using Microsoft.EntityFrameworkCore;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        // Add here any methods that you need for the repository, basic CRUD operations are already implemented in the generic repository

        // Access context via DbContext and Entities

        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var result = await Entities.Where(u => u.Email == email).FirstOrDefaultAsync();
            return result;
        }
    }
}