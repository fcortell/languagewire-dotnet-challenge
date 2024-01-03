using UserService.Domain.Users.Entities;

namespace UserService.Tests.Shared
{
	public class UserBuilder
	{
		private string email = UniqueEmailGenerator.Generate();
		private string name = "Testy";

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