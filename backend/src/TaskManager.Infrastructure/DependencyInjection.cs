using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Infrastructure;

/// <summary>
/// Dependency injection extensions for Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<DatabaseInitializer>();

        return services;
    }

    /// <summary>
    /// Initializes the database schema
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await initializer.InitializeAsync();
    }
}
