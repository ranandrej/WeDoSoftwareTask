using Application.Common;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DependencyInjection
{
    public static class ServiceContainer
    {
            public static IServiceCollection ApplicationServices(this IServiceCollection services)

            {
                services.AddScoped<IAuthService, AuthService>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IWorkoutService, WorkoutService>();
                services.AddScoped<WorkoutValidator>();

            return services;
            }
        }

    
}
