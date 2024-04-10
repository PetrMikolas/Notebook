using Notebook.Exceptions;
using Notebook.Models;

namespace Notebook.Repositories.Notebook;

/// <summary>
/// Implements repository operations for managing sections and pages.
/// </summary>
public interface INotebookRepository
{
    /// <summary>
    /// Retrieves the sections associated with a specific user identified by his ID.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of sections.</returns>
    Task<List<Section>> GetSectionsAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new section.
    /// </summary>
    /// <param name="entity">The section entity to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task CreateSectionAsync(Section? entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing section associated with a specific user identified by his ID.
    /// </summary>
    /// <param name="entity">The section entity to update.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="EntityNotFoundException">Thrown when the section is not found.</exception>
    Task UpdateSectionAsync(Section? entity, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a section by its identifier and associated with a specific user identified by his ID.
    /// </summary>
    /// <param name="id">The section identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="EntityNotFoundException">Thrown when the section is not found.</exception>
    Task DeleteSectionAsync(int id, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the content of a page by its identifier and associated with a specific user identified by his ID.
    /// </summary>
    /// <param name="id">The page identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The content of the page.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the page is not found.</exception>
    Task<string> GetPageContentById(int id, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new page to a section associated with a specific user identified by his ID.
    /// </summary>
    /// <param name="entity">The page entity to add.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="EntityNotFoundException">Thrown when the page is not found.</exception>
    Task AddPageAsync(Page? entity, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing page associated with a specific user identified by his ID.
    /// </summary>
    /// <param name="entity">The page entity to update.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="EntityNotFoundException">Thrown when the page is not found.</exception>
    Task UpdatePageAsync(Page? entity, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a page by its identifier and associated with a specific user identified by his ID.
    /// </summary>
    /// <param name="id">The page identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="EntityNotFoundException">Thrown when the page is not found.</exception>
    Task DeletePageAsync(int id, string userId, CancellationToken cancellationToken);
}