using UserService.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Persistence;

namespace UserService.API.Setup;

public static class PersistenceSetup
{
	public static void EnsurePersistenceIsReady(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		dbContext.Database.EnsureCreated();
	}
}
