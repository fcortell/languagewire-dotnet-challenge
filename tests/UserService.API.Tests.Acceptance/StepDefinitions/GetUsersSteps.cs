using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Users.Commands.CreateUser;
using UserService.Application.Users.Queries;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;
using UserService.Tests.Shared.Users;
using UserService.Tests.Shared.Utils;

namespace UserService.API.Test.Acceptance.StepDefinitions;

[Binding]
public class UserSteps : StepsBase
{

    public UserSteps(ScenarioContext scenarioContext, TestFixture fixture) : base(scenarioContext, fixture) { }

    public const string GivenUsersKey = "GivenUsers";
    public const string RequestBodyKey = "RequestBody";
    public const string HttpResponseKey = "HttpResponse";
    public const string IdKey = "Id";

    [Given(@"an existing user")]
    public async Task GivenAnExistingUser()
    {
        var givenUsers = new List<User>();
        if (_scenarioContext.ContainsKey(GivenUsersKey))
            givenUsers.AddRange((List<User>)_scenarioContext[GivenUsersKey]);

        var givenUser = new UserBuilder()
            .Build();

        await SaveEntity<User>(givenUser);

        givenUsers.Add(givenUser);
        _scenarioContext[GivenUsersKey] = givenUsers;
        _scenarioContext[IdKey] = givenUser.Id;
    }

    [Given(@"two existing users")]
    public async Task GivenTwoExistingUsers()
    {
        await GivenAnExistingUser();
        await GivenAnExistingUser();
    }

    [Given(@"a UserCreationRequest with the same email")]
    public void GivenAUserCreationRequestWithTheSameEmail()
    {
        var user = ((List<User>)_scenarioContext[GivenUsersKey]).First();
        var givenRequest = new CreateUserCommand()
        {
            Email = user.Email,
            Name = "Test User"
        };

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [Given(@"a UserCreationRequest")]
    public void GivenAUserCreationRequest()
    {
        var givenRequest = new CreateUserCommand()
        {
            Email = "test@test33.com",
            Name = "Test User"
        };

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [When(@"I send a (.*) request to (.*)")]
    public async Task ISendARequestTo(string verb, string path)
    {
        if (path.Contains("{id}"))
            path = path.Replace("{id}", _scenarioContext[IdKey].ToString());

        var request = new HttpRequestMessage(new HttpMethod(verb), path);

        if (verb.ToLower() == "post")
            request.Content = JsonContent.Create(_scenarioContext[RequestBodyKey]);

        var response = await _fixture.HttpClient.SendAsync(request);

        _scenarioContext[HttpResponseKey] = response;
    }

    [Then(@"I get a Conflict response with an error message")]
    public void ThenIGetAConflictReponseWithAnErrorMessage()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var contentStream = httpResponse.Content.ReadAsStream();
        var response = JsonSerializer.Deserialize<ProblemDetails>(contentStream, _jsonOptions);
        response.Should().NotBeNull();
        response!.Status.Should().BeGreaterThan(400);
        response!.Title.Should().NotBeNullOrWhiteSpace();
    }

    [Then(@"I get an OK response")]
    public void ThenIGetAnOkResponse()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"I get the users")]
    public void ThenIGetTheUsers()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var contentStream = httpResponse.Content.ReadAsStream();
        var response = JsonSerializer.Deserialize<IEnumerable<UserDTO>>(contentStream, _jsonOptions);
        response.Should().NotBeNull();

        var givenUsers = (List<User>)_scenarioContext[GivenUsersKey];
        foreach (var user in givenUsers)
        {
            var userResponse = response!.FirstOrDefault(r => r.Id == user.Id);
            userResponse.Should().NotBeNull();
            AssertEquivalent(userResponse!, user);
        }
    }

    [Then(@"I get the user")]
    public void ThenIGetTheUser()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var contentStream = httpResponse.Content.ReadAsStream();
        var response = JsonSerializer.Deserialize<UserDTO>(contentStream, _jsonOptions);
        response.Should().NotBeNull();

        var givenUser = ((List<User>)_scenarioContext[GivenUsersKey]).First();
        AssertEquivalent(response!, givenUser);

    }

    [Then(@"the user is persisted")]
    public async Task ThenTheUserIsPersisted()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];
        var content = await httpResponse.Content.ReadAsStringAsync();
        var id = long.Parse(content);

        var user = await GetEntity<User>(id);
        user.Should().NotBeNull();
        var userCreationRequest = (CreateUserCommand)_scenarioContext[RequestBodyKey];
        userCreationRequest.Should().NotBeNull();
        AssertEquivalent(userCreationRequest!, user!);
    }

    private static void AssertEquivalent(UserDTO response, User user)
    {
        response.Id.Should().Be(user.Id);
        response.Name.Should().Be(user.Name);
        response.Email.Should().Be(user.Email);
    }

    private static void AssertEquivalent(CreateUserCommand request, User user)
    {
        request.Name.Should().Be(user.Name);
        request.Email.Should().Be(user.Email);
    }
}