using UserService.API.Setup;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UserService.Infrastructure;
using UserService.Application;
using Microsoft.AspNetCore.Diagnostics;
using UserService.API.Middleware;

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
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();
app.EnsurePersistenceIsReady();

Configuration = app.Configuration;

app.Run();

public partial class Program
{
	public static IConfiguration? Configuration { get; private set; }
	public static bool EnableBackgroundJobs = true;
}