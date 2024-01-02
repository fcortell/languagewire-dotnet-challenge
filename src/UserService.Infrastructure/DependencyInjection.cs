using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Common.Interfaces;
using UserService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Users;
using UserService.Infrastructure.Persistence.Repositories;
using UserService.Infrastructure.Persistence;

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

            services.AddTransient<IDateTime, DateTimeService>();

            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            return services;
        }

    }
}
