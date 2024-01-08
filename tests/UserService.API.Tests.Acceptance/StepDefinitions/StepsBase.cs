using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Text.Json;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;
using UserService.Infrastructure.Persistence.Repositories;

namespace UserService.API.Test.Acceptance.StepDefinitions;

public class StepsBase
{
    protected readonly ScenarioContext _scenarioContext;
    protected readonly TestFixture _fixture;

    protected readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public StepsBase(ScenarioContext scenarioContext, TestFixture fixture)
    {
        _scenarioContext = scenarioContext;
        _fixture = fixture;
    }

    protected async Task SaveEntity<T>(User entity) where T : class
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        repository.Insert(entity);
        await repository.CommitChangesAsync(System.Threading.CancellationToken.None);
    }

    protected async Task<User?> GetEntity<T>(long id) where T : class
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        return await repository.GetByIdAsync(id);
    }
}
