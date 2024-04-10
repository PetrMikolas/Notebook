using Notebook.Models;

namespace Notebook.Services.Notebook;

/// <summary>
/// Interface for managing notebook-related operations.
/// </summary>
public interface INotebookService
{
    /// <summary>
    /// Gets all sections asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of sections.</returns>
    Task<List<Section>> GetSectionsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Searches sections asynchronously based on the provided search text.
    /// </summary>
    /// <param name="searchText">The search text.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of sections matching the search criteria.</returns>
    Task<List<Section>> SearchSectionsAsync(string searchText, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new section asynchronously.
    /// </summary>
    /// <param name="entity">The section entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task CreateSectionAsync(Section? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing section asynchronously.
    /// </summary>
    /// <param name="entity">The section entity to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task UpdateSectionAsync(Section? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a section asynchronously.
    /// </summary>
    /// <param name="id">The ID of the section to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task DeleteSectionAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new page to a section asynchronously.
    /// </summary>
    /// <param name="entity">The page entity to be added.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task AddPageAsync(Page? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing page asynchronously.
    /// </summary>
    /// <param name="entity">The page entity to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task UpdatePageAsync(Page? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a page asynchronously.
    /// </summary>
    /// <param name="id">The ID of the page to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task DeletePageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the content of a page by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The content of the page.</returns>
    Task<string> GetPageContentById(int id, CancellationToken cancellationToken);
}