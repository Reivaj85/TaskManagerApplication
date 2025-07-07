using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.UseCases.Authentication;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Application.UseCases.Users;

namespace TaskManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register use case services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();

        return services;
    }
}
