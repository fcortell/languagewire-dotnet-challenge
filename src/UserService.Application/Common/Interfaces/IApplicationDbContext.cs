using Microsoft.EntityFrameworkCore;
using UserService.Domain.Users.Entities;

namespace UserService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}