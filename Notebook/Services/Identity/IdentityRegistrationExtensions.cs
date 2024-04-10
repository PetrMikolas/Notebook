using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Notebook.Account;
using Notebook.Databases.Notebook;
using Notebook.LocalizedIdentityError;
using Notebook.Models;

namespace Notebook.Services.Identity;

/// <summary>
/// Extension methods for registering identity-related services.
/// </summary>
public static class IdentityRegistrationExtensions
{
    /// <summary>
    /// Adds identity services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to which the identity services will be added.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<NotebookDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>();

        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

        return services;
    }
}