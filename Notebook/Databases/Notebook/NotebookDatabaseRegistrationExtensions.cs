using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notebook.Repositories.Notebook;
using Notebook.Services.Email;

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
    /// <param name="email">The service for sending email notifications.</param>
    /// <returns>The collection of services with added database services.</returns>
    public static IServiceCollection AddNotebookDatabase(this IServiceCollection services, IConfiguration configuration, IEmailService email)
    {
        var connectionString = configuration.GetConnectionString("Notebook");

        if (string.IsNullOrEmpty(connectionString))
        {
            string errorMessage = "Nelze získat connection string na připojení databáze Notebook";
            Type classType = typeof(NotebookDatabaseRegistrationExtensions);

            _ = email.SendErrorAsync(errorMessage, classType, nameof(AddNotebookDatabase));
            LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger(classType).LogError(errorMessage);

            return services;
        }

        services.AddDbContext<NotebookDbContext>(options =>
        {
            options.UseSqlServer(connectionString, opts =>
            {
                opts.MigrationsHistoryTable("MigrationHistory_Notebook");
            });
        });
             
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.RemoveAll<INotebookRepository>();
        services.AddScoped<INotebookRepository, NotebookRepository>();

        return services;
    }

    /// <summary>
    /// Applies pending migrations to the database.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    /// <param name="email">The service for sending email notifications.</param>
    /// <returns>The configured web application builder.</returns>
    public static WebApplication UseNotebookDatabase(this WebApplication app, IEmailService email)
    {
        if (app.Environment.EnvironmentName != "IntegrationTests")
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<NotebookDbContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                _ = email.SendErrorAsync(ex.ToString());
                app.Logger.LogError(ex.ToString());

                return app;
            }
        }

        return app;
    }
}