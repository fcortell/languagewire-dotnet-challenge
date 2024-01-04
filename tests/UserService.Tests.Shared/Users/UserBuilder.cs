using UserService.Domain.Users.Entities;
using UserService.Tests.Shared.Utils;

namespace UserService.Tests.Shared.Users
{
    public class UserBuilder
    {
        private string email = UniqueEmailGenerator.Generate();
        private string name = "Testy" + new Random().Next(int.MinValue, int.MaxValue);

        public User Build() => new User
        {
            Email = email,
            Name = name
        };

        public UserBuilder WithEmail(string val)
        {
            email = val;
            return this;
        }

        public UserBuilder WithName(string val)
        {
            name = val;
            return this;
        }
    }
}