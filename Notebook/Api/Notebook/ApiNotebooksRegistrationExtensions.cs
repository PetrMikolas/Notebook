using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notebook.Exceptions;
using Notebook.Helpers;
using Notebook.Models;
using Notebook.Services.Notebook;
using Notebook.Shared.Exceptions;

namespace Notebook.Api.Notebook;

/// <summary>
/// Provides extension methods for mapping endpoints related to notebook operations in the API.
/// </summary>
public static class ApiNotebooksRegistrationExtensions
{
    /// <summary>
    /// Maps endpoints related to notebook operations in the API.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application with mapped endpoints for notebook operations.</returns>
    public static WebApplication MapEndpointsNotebook(this WebApplication app)
    {
        app.MapGet("sections", async ([FromServices] INotebookService notebookService, [FromServices] IMapper mapper, CancellationToken cancellationToken) =>
        {
            try
            {
                var sections = await notebookService.GetSectionsAsync(cancellationToken);
                return Results.Ok(sections.Select(mapper.Map<SectionDto>).ToList());
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Sections")
        .WithName("GetSections")
        .WithOpenApi(operation => new(operation) { Summary = "Get sections and pages without content" })
        .Produces<List<SectionDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("sections/search/{text}", async ([FromRoute] string text, [FromServices] INotebookService notebookService, [FromServices] IMapper mapper, CancellationToken cancellationToken) =>
        {
            try
            {
                var sections = await notebookService.SearchSectionsAsync(text, cancellationToken);
                return Results.Ok(sections.Select(mapper.Map<SectionDto>).ToList());
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Sections")
        .WithName("SearchValues")
        .WithOpenApi(operation => new(operation) { Summary = "Search sections and pages without content" })
        .Produces<List<SectionDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("sections", async ([FromBody] SectionDto sectionDto, [FromServices] INotebookService notebookService, [FromServices] IMapper mapper, CancellationToken cancellationToken) =>
        {
            if (!Helper.IsValidSectionDto(sectionDto, HttpMethod.Post, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entity = mapper.Map<Section>(sectionDto);
                await notebookService.CreateSectionAsync(entity, cancellationToken);
                return Results.Created($"sections/{entity?.Id}", sectionDto);
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Sections")
        .WithName("CreateSection")
        .WithOpenApi(operation => new(operation) { Summary = "Create an section" })
        .Produces<SectionDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("sections", async ([FromBody] SectionDto sectionDto, [FromServices] INotebookService notebookService, [FromServices] IMapper mapper, CancellationToken cancellationToken) =>
        {
            if (!Helper.IsValidSectionDto(sectionDto, HttpMethod.Put, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entita = mapper.Map<Section>(sectionDto);
                await notebookService.UpdateSectionAsync(entita, cancellationToken);
                return Results.NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Sections")
        .WithName("UpdateSection")
        .WithOpenApi(operation => new(operation) { Summary = "Update the section" })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("sections/{id}", async ([FromRoute] int id, [FromServices] INotebookService notebookService, CancellationToken cancellationToken) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest($"Parametr <{nameof(id)}> musí mít větší hodnotu než nula");
            }

            try
            {
                await notebookService.DeleteSectionAsync(id, cancellationToken);
                return Results.NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Sections")
        .WithName("DeleteSection")
        .WithOpenApi(operation => new(operation) { Summary = "Delete section by ID" })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("pages", async ([FromBody] PageDto pageDto, [FromServices] INotebookService notebookService, [FromServices] IMapper mapper, CancellationToken cancellationToken) =>
        {
            if (!Helper.IsValidPageDto(pageDto, HttpMethod.Post, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entity = mapper.Map<Page>(pageDto);
                await notebookService.AddPageAsync(entity, cancellationToken);
                return Results.Created($"pages/{entity?.Id}", pageDto);
            }
            catch (EntityNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Pages")
        .WithName("AddPage")
        .WithOpenApi(operation => new(operation) { Summary = "Adds new page to the section" })
        .Produces<SectionDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("pages", async ([FromBody] PageDto pageDto, [FromServices] INotebookService notebookService, [FromServices] IMapper mapper, CancellationToken cancellationToken) =>
        {
            if (!Helper.IsValidPageDto(pageDto, HttpMethod.Put, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entita = mapper.Map<Page>(pageDto);
                await notebookService.UpdatePageAsync(entita, cancellationToken);
                return Results.NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Pages")
        .WithName("UpdatePage")
        .WithOpenApi(operation => new(operation) { Summary = "Update the page" })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("pages/{id}", async ([FromRoute] int id, [FromServices] INotebookService notebookService, CancellationToken cancellationToken) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest($"Parametr <{nameof(id)}> musí být větší než nula");
            }

            try
            {
                await notebookService.DeletePageAsync(id, cancellationToken);
                return Results.NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Pages")
        .WithName("DeletePage")
        .WithOpenApi(operation => new(operation) { Summary = "Delete page by ID" })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("pages/content/{id}", async ([FromRoute] int id, [FromServices] INotebookService notebookService, CancellationToken cancellationToken) =>
        {
            try
            {
                var content = await notebookService.GetPageContentById(id, cancellationToken);
                return Results.Ok(content);
            }
            catch (EntityNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (NotAuthorizedException)
            {
                return Results.Unauthorized();
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithTags("Pages")
        .WithName("GetPageContent")
        .WithOpenApi(operation => new(operation) { Summary = "Get page content by ID" })
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}