using Microsoft.AspNetCore.Mvc;
using Notebook.Services.Email;

namespace Notebook.Api.ErrorClient;

/// <summary>
/// Provides extension methods for mapping endpoints related to error handling and reporting from the client to the server.
/// </summary>
public static class ApiErrorsClientRegistrationExtensions
{
    /// <summary>
    /// Maps an endpoint for reporting errors from the client to the server.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application with mapped endpoint for error reporting.</returns>
    public static WebApplication MapEndpointsErrorClient(this WebApplication app)
    {
        app.MapPost("errors", ([FromBody] string errorMessage, [FromServices] IEmailService email, CancellationToken cancellationToken) =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {errorMessage}");            
            Console.ResetColor();

            _ = email.SendErrorAsync(errorMessage, cancellationToken);

            return Results.NoContent();
        })
        .WithTags("Errors")
        .WithName("SendError")
        .WithOpenApi(operation => new(operation) { Summary = "Send an error" })
        .Produces(StatusCodes.Status204NoContent);

        return app;
    }
}