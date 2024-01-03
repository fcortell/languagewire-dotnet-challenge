using System.Net.Http.Json;
using System.Text.RegularExpressions;
using FluentResults;
using UserService.Domain.Users.Entities;

namespace UserService.Application;

public class UserCreator
{
	public async Task<Result<long>> Create(User model)
	{
        User user = model;

        var nameResult = model.Name;

		//if (nameResult.IsFailure)
			//return Result.Failure<long, Error>(new Error(nameResult.Error));

		if (!Regex.IsMatch(model.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
			throw new Exception("Email is invalid");

		//UserOld.Repository = new UserRepository(repository); // Get user repository ready in User

		//try
		//{
		//	user = UserOld.Create(nameResult.Value, model.Email);
		//}
		//catch (Exception ex)
		//{
		//	return Result.Failure<long, Error>(
		//	new UniqueConstraintViolationError(
		//		"User with given Email already exists.", nameof(User), nameof(User.Email)));
		//}

		RegisterInCrm(user);

		return Result.Ok<long>(user.Id);
	}

	private void RegisterInCrm(User user)
	{
		var httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
		var message = new HttpRequestMessage(HttpMethod.Post, "users");
		message.Content = JsonContent.Create(new { Name = user.Name, Email = user.Email });
		httpClient.Send(message);
	}
}