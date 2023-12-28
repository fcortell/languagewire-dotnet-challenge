namespace UserService.Domain;

[Obsolete("Not used any more", true)]
public class UserOld : Entity
{
	public static IUserRepository Repository { get; set; }
	public Name Name { get; protected set; }
	public string Email { get; protected set; }

	public UserOld(Name name, string email)
	{
		Name = name;
		Email = email;
	}

	public static UserOld Create(Name name, string email)
	{
		var allUsers = Repository.GetAll();
		if (allUsers.Any(u => u.Email == email))
			throw new Exception("Email is duplicated");

		var user = new UserOld(name, email);
		Repository.Add(user);
		Repository.Save();
		return user;
	}

	protected UserOld() { }
}