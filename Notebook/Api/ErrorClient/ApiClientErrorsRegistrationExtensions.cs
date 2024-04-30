﻿using Microsoft.AspNetCore.Mvc;
using Notebook.Services.Email;

namespace Notebook.Api.ErrorClient;

/// <summary>
/// Extension method for registering client errors API endpoints.
/// </summary>
public static class ApiClientErrorsRegistrationExtensions
{
    /// <summary>
    /// Maps an endpoint for reporting client errors to the server.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application with mapped endpoint for reporting errors.</returns>
    public static WebApplication MapEndpointsClientErrors(this WebApplication app)
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