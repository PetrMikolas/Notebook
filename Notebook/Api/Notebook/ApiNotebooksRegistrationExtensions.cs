using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notebook.Exceptions;
using Notebook.Models;
using Notebook.Services.Notebook;
using Notebook.Shared.Exceptions;
using System.Security.Claims;

namespace Notebook.Api.Notebook;

/// <summary>
/// Extension method for registering notebook API endpoints.
/// </summary>
public static class ApiNotebooksRegistrationExtensions
{
    /// <summary>
    /// Maps endpoints related to notebook operations in the API.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application with mapped endpoints for notebook operations.</returns>
    public static WebApplication MapNotebookEndpoints(this WebApplication app)
    {
        app.MapGet("sections", async (
            ClaimsPrincipal user,
            [FromServices] INotebookService notebookService,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;                              

                var sections = await notebookService.GetSectionsAsync(userId, cancellationToken);
                return Results.Ok(sections.Select(mapper.Map<SectionDto>));
            }            
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .RequireAuthorization()
        .WithTags("Sections")
        .WithName("GetSections")       
        .Produces<IEnumerable<SectionDto>>(StatusCodes.Status200OK)        
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("sections/search/{text}", async (
            ClaimsPrincipal claimsPrincipal,
            [FromRoute] string text, 
            [FromServices] INotebookService notebookService, 
            [FromServices] IMapper mapper, 
            CancellationToken cancellationToken) =>
        {
            try
            {
                var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;

                var sections = await notebookService.SearchSectionsAndPagesWithMatchesAsync(userId, text, cancellationToken);
                return Results.Ok(sections.Select(mapper.Map<SectionDto>));
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
        .RequireAuthorization()
        .WithTags("Sections")
        .WithName("SearchSectionsAndPages")       
        .Produces<IEnumerable<SectionDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("sections", async (
            ClaimsPrincipal user,
            [FromBody] SectionDto sectionDto, 
            [FromServices] INotebookService notebookService, 
            [FromServices] IMapper mapper, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (!IsValidSectionDto(sectionDto, HttpMethod.Post, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entity = mapper.Map<Section>(sectionDto);
                await notebookService.CreateSectionAsync(userId, entity, cancellationToken);
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
        .RequireAuthorization()
        .WithTags("Sections")
        .WithName("CreateSection")        
        .Produces<SectionDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("sections", async (
            ClaimsPrincipal user,
            [FromBody] SectionDto sectionDto, 
            [FromServices] INotebookService notebookService, 
            [FromServices] IMapper mapper, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (!IsValidSectionDto(sectionDto, HttpMethod.Put, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entita = mapper.Map<Section>(sectionDto);
                await notebookService.UpdateSectionAsync(userId, entita, cancellationToken);
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
        .RequireAuthorization()
        .WithTags("Sections")
        .WithName("UpdateSection")       
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("sections/{id}", async (
            ClaimsPrincipal user,
            [FromRoute] int id, 
            [FromServices] INotebookService notebookService, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (id <= 0)
            {
                return Results.BadRequest($"Parametr <{nameof(id)}> musí mít větší hodnotu než nula");
            }

            try
            {
                await notebookService.DeleteSectionAsync(userId, id, cancellationToken);
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
        .RequireAuthorization()
        .WithTags("Sections")
        .WithName("DeleteSection")        
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("pages", async (
            ClaimsPrincipal user,
            [FromBody] PageDto pageDto, 
            [FromServices] INotebookService notebookService, 
            [FromServices] IMapper mapper, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (!IsValidPageDto(pageDto, HttpMethod.Post, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entity = mapper.Map<Page>(pageDto);
                await notebookService.AddPageAsync(userId, entity, cancellationToken);
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
        .RequireAuthorization()
        .WithTags("Pages")
        .WithName("AddPage")        
        .Produces<SectionDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("pages", async (
            ClaimsPrincipal user,
            [FromBody] PageDto pageDto, 
            [FromServices] INotebookService notebookService, 
            [FromServices] IMapper mapper, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (!IsValidPageDto(pageDto, HttpMethod.Put, out string error))
            {
                return Results.BadRequest(error);
            }

            try
            {
                var entita = mapper.Map<Page>(pageDto);
                await notebookService.UpdatePageAsync(userId, entita, cancellationToken);
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
        .RequireAuthorization()
        .WithTags("Pages")
        .WithName("UpdatePage")        
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("pages/{id}", async (
            ClaimsPrincipal user,
            [FromRoute] int id, 
            [FromServices] INotebookService notebookService, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (id <= 0)
            {
                return Results.BadRequest($"Parametr <{nameof(id)}> musí být větší než nula");
            }

            try
            {
                await notebookService.DeletePageAsync(userId, id, cancellationToken);
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
        .RequireAuthorization()
        .WithTags("Pages")
        .WithName("DeletePage")        
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("pages/content/{id}", async (
            ClaimsPrincipal user,
            [FromRoute] int id, 
            [FromServices] INotebookService notebookService, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var content = await notebookService.GetPageContentById(userId, id, cancellationToken);
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
        .RequireAuthorization()
        .WithTags("Pages")
        .WithName("GetPageContent")        
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }

    /// <summary>
    /// Checks if the provided SectionDto object is valid.
    /// </summary>
    /// <param name="sectionDto">The SectionDto object to validate.</param>
    /// <param name="httpMethod">The HTTP method used.</param>
    /// <param name="error">The error message if validation fails.</param>
    /// <returns>True if the SectionDto object is valid; otherwise, false.</returns>
    private static bool IsValidSectionDto(SectionDto? sectionDto, HttpMethod httpMethod, out string error)
    {
        error = string.Empty;

        if (sectionDto is null)
        {
            error = $"Parametr <{nameof(sectionDto)}> nemůže být null.";
            return false;
        }

        if (httpMethod == HttpMethod.Post && sectionDto.Id != 0)
        {
            error = $"Parametr <{nameof(sectionDto.Id)}> musí mít hodnotu 0. ";
        }

        if (httpMethod != HttpMethod.Post && sectionDto.Id <= 0)
        {
            error = $"Parametr <{nameof(sectionDto.Id)}> musí mít hodnotu větší než 0. ";
        }

        if (sectionDto.Name is null)
        {
            error += $"Parametr <{nameof(sectionDto.Name)}> nemůže být null. ";
        }
        else if (sectionDto.Name.Length <= 0 || sectionDto.Name.Length > 30)
        {
            error += $"Parametr <{nameof(sectionDto.Name)}> může mít délku 1 až 30 znaků. ";
        }

        return error == string.Empty;
    }

    /// <summary>
    /// Checks if the provided PageDto object is valid.
    /// </summary>
    /// <param name="pageDto">The PageDto object to validate.</param>
    /// <param name="httpMethod">The HTTP method used.</param>
    /// <param name="error">The error message if validation fails.</param>
    /// <returns>True if the PageDto object is valid; otherwise, false.</returns>
    private static bool IsValidPageDto(PageDto? pageDto, HttpMethod httpMethod, out string error)
    {
        error = string.Empty;

        if (pageDto is null)
        {
            error = $"Parametr <{nameof(pageDto)}> nemůže být null.";
            return false;
        }

        if (pageDto.SectionId <= 0)
        {
            error = $"Parametr <{nameof(pageDto.SectionId)}> musí mít hodnotu větší než 0. ";
        }

        if (httpMethod == HttpMethod.Post && pageDto.Id != 0)
        {
            error += $"Parametr <{nameof(pageDto.Id)}> musí mít hodnotu 0. ";
        }

        if (httpMethod != HttpMethod.Post && pageDto.Id <= 0)
        {
            error += $"Parametr <{nameof(pageDto.Id)}> musí mít hodnotu větší než 0. ";
        }

        if (pageDto.Title is null)
        {
            error += $"Parametr <{nameof(pageDto.Title)}> nemůže být null. ";
        }
        else if (pageDto.Title.Length <= 0 || pageDto.Title.Length > 30)
        {
            error += $"Parametr <{nameof(pageDto.Title)}> může mít délku 1 až 30 znaků. ";
        }

        if (pageDto.Content is null)
        {
            error += $"Parametr <{nameof(pageDto.Content)}> nemůže být null";
        }

        return error == string.Empty;
    }
}