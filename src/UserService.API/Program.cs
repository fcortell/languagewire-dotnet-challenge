using UserService.API.ErrorResponseHandling;
using UserService.API.Setup;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UserService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
	.AddUserSecrets<Program>()
	.AddEnvironmentVariables();

builder.Services.AddControllers(options =>
	{
#if !DEBUG
		options.Filters.Add<ExceptionHandlingFilter>();
#endif
	})
	.ConfigureApiBehaviorOptions(options =>
	{
		options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.Create;
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.EnsurePersistenceIsReady();

Configuration = app.Configuration;

app.Run();

public partial class Program
{
	public static IConfiguration? Configuration { get; private set; }
	public static bool EnableBackgroundJobs = true;
}