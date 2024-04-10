using Notebook.Services.User;

namespace Notebook.Middlewares;

/// <summary>
/// Middleware for fetching information about the current user.
/// </summary>
/// <param name="userService">The service responsible for fetching user information.</param>
/// <param name="logger">The logger.</param>
public sealed class UserMiddleware(IUserService userService, ILogger<UserMiddleware> logger) : IMiddleware
{
    /// <summary>
    /// Invokes the middleware to fetch information about the current user.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			userService.FetchCurrentUser(context.User);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Chyba při získávání informací o uživateli.");

			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync(ex.Message);
			return;
		}

		await next(context);
	}
}