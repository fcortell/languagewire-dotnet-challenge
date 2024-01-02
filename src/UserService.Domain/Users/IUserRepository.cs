using UserService.Domain.Users.Entities;

namespace UserService.Domain.Users;

public interface IUserRepository : IRepository<User>
{
    // Add here any methods that you need for the repository, basic CRUD operations are already implemented in the generic repository
}