using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using UserService.Infrastructure.Persistence;

namespace UserService.API.Test.Acceptance;

public class TestFixture
{
    public readonly HttpClient HttpClient;
    public readonly MockHttpMessageHandler MockHttpMessageHandler;
    public readonly IServiceProvider ServiceProvider;

    public TestFixture()
    {
        Program.EnableBackgroundJobs = false;
        var applicationFactory = new CustomWebApplicationFactory();
        HttpClient = applicationFactory.CreateClient();
        ServiceProvider = applicationFactory.Services;
        MockHttpMessageHandler = applicationFactory.MockHttpMessageHandler;

        MigrateDb();
    }

    private void MigrateDb()
    {
        if (!Program.Configuration!.GetConnectionString("Database")!.Contains("Server=.,8003"))
            return;

        using var scope = ServiceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}