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
using UserService.Infrastructure.Persistence;
using UserService.Application.Ports;
using UserService.Application;
using UserService.Infrastructure.EntityFramework;

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

            services.AddTransient<IDateTime, DateTimeService>();

            services.AddScoped(typeof(IAsyncRepository), typeof(EfAsyncRepository));
            services.AddScoped<UserCreator>();

            return services;
        }

    }
}
