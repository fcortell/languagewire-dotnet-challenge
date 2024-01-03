using UserService.Domain.Users.Entities;

namespace UserService.Domain.Users;

public interface IUserRepository : IGenericRepository<User>
{
    // Add here any methods that you need for the repository, basic CRUD operations are already implemented in the generic repository
    Task<User?> GetByEmailAsync(string email);
}