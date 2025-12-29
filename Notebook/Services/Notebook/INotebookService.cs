using Notebook.Models;

namespace Notebook.Services.Notebook;

/// <summary>
/// Interface for managing notebook-related operations.
/// </summary>
public interface INotebookService
{
    /// <summary>
    /// Retrieves all sections and their associated pages asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable collection of sections, each including its associated pages.</returns>
    Task<IEnumerable<Section>> GetSectionsAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously searches for sections and their pages based on the provided search text.
    /// If the search text matches a section's name, the section is included in the results along with its pages.
    /// If the search text matches a page's title, the entire section containing the page is included in the results.
    /// Sections that contain both matching titles and pages will be included in the results only once.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="searchText">The text to search for within section names and page titles.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable collection of sections that match the search criteria. Each section included in the results will also include its pages if the section or any of its pages match the search text.</returns>
    Task<IEnumerable<Section>> SearchSectionsAndPagesWithMatchesAsync(string userId, string searchText, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new section asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="entity">The section entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task CreateSectionAsync(string userId, Section? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing section asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="entity">The section entity to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task UpdateSectionAsync(string userId, Section? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a section asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id">The ID of the section to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task DeleteSectionAsync(string userId, int id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new page to a section asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="entity">The page entity to be added.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task AddPageAsync(string userId, Page? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing page asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="entity">The page entity to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task UpdatePageAsync(string userId, Page? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a page asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id">The ID of the page to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task DeletePageAsync(string userId, int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the content of a page by ID asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id">The ID of the page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The content of the page.</returns>
    Task<string> GetPageContentById(string userId, int id, CancellationToken cancellationToken);
}