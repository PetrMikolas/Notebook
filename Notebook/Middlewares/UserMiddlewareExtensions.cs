namespace Notebook.Middlewares;

/// <summary>
/// Provides extension methods to register the UserMiddleware in the application pipeline.
/// </summary>
public static class UserMiddlewareExtensions
{
    /// <summary>
    /// Registers the UserMiddleware in the application pipeline to handle user-related requests.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder app)
	{
        app.UseWhen(context =>
            context.Request.Path.HasValue &&
            (context.Request.Path.Value.StartsWith("/sections") || context.Request.Path.Value.StartsWith("/pages")),
            appBuilder => appBuilder.UseMiddleware<UserMiddleware>());

        return app;
    }
}