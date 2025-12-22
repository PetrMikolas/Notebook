using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notebook.Exceptions;
using Notebook.Helpers;
using Notebook.Repositories.Notebook;

namespace Notebook.Databases.Notebook;

/// <summary>
/// Provides extension methods for configuring database services related to the notebook module.
/// </summary>
public static class NotebookDatabaseRegistrationExtensions
{
    /// <summary>
    /// Registers database services related to the notebook module in the dependency injection container.
    /// </summary>
    /// <param name="services">The collection of services in the application.</param>
    /// <param name="configuration">The configuration of the application.</param>
    /// <returns>The collection of services with added database services.</returns>
    public static IServiceCollection AddNotebookDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Notebook");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ConnectionStringNotFoundException("Nelze získat connection string na připojení databáze Notebook");

        services.AddDbContext<NotebookDbContext>(options =>
        {
            options.UseSqlServer(connectionString, opts =>
            {
                opts.MigrationsHistoryTable("MigrationHistory_Notebook");
            });
        });

        services.RemoveAll<INotebookRepository>();
        services.AddScoped<INotebookRepository, NotebookRepository>();

        return services;
    }

    /// <summary>
    /// Applies pending migrations to the database.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>
    public static async Task<WebApplication> UseNotebookDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NotebookDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync(app.Lifetime.ApplicationStopping);
        }
        catch (Exception ex)
        {
            app.CreateLogger().LogError(ex, "Database migration failed");
            throw;
        }

        return app;
    }
}