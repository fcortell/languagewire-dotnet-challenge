namespace UserService.API.Setup;

public static class PersistenceSetup
{
    public static void EnsurePersistenceIsReady(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Persistence.ApplicationDbContext>();
        dbContext.Database.EnsureCreated();
    }
}