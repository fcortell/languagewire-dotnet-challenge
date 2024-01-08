using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Tiers;
using UserService.Domain.Users;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Persistence.CacheRepositories;
using UserService.Infrastructure.Persistence.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<Persistence.ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("Database"));
            }
            else
            {
                services.AddDbContext<Persistence.ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("Database"),
                        options
                        => options.EnableRetryOnFailure()));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddMemoryCache();
            services.AddTransient<IDateTime, DateTimeService>();

            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped<TierRepository>();
            services.AddScoped<ITierRepository>(provider =>
            {
                var tierRepository = provider.GetService<TierRepository>()!;
                return new CachedTierRepository(tierRepository, provider.GetService<IMemoryCache>()!);
            });
            return services;
        }
    }
}