namespace Notebook.Client.Services.Api;

/// <summary>
/// Represents an interface for interacting with an API service to manage notebook sections and pages.
/// </summary>
public interface IApiService
{
    /// <summary>
    /// Retrieves all sections asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of SectionDto objects.</returns>
    Task<List<SectionDto>> GetSectionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for sections or pages matching the provided search text asynchronously.
    /// </summary>
    /// <param name="searchText">The text to search for.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of SectionDto objects.</returns>
    Task<List<SectionDto>> SearchValuesAsync(string searchText, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new section asynchronously.
    /// </summary>
    /// <param name="section">The section object to be created.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task CreateSectionAsync(SectionDto section, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing section asynchronously.
    /// </summary>
    /// <param name="section">The section object to be updated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task UpdateSectionAsync(SectionDto section, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a section asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the section to be deleted.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task DeleteSectionAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the content of a page asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the page whose content is to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the content of the page as a string.</returns>
    Task<string> GetPageContentAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new page asynchronously.
    /// </summary>
    /// <param name="page">The page object to be added.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task AddPageAsync(PageDto page, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing page asynchronously.
    /// </summary>
    /// <param name="page">The page object to be updated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task UpdatePageAsync(PageDto page, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a page asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the page to be deleted.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    Task DeletePageAsync(int id, CancellationToken cancellationToken = default);
}