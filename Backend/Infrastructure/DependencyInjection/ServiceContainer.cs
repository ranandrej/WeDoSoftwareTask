using Application.Interfaces;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection InfrastructureServices(
        this IServiceCollection services,
        IConfiguration config)
        {
            services.AddDbContext<TrainingAppDBContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            

            // Kad god neko trazi interfejs,mapiraj na klasu i innstacira objekat te klase da bi moglo da se koristi.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            return services;
        }
    }
}
