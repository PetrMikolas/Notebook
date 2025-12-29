using Microsoft.Extensions.Caching.Memory;
using Notebook.Models;
using Notebook.Repositories.Notebook;
using System.Globalization;
using System.Text;

namespace Notebook.Services.Notebook;

/// <summary>
/// Service for managing notebook sections and pages based on the <seealso cref="INotebookService"/> interface.
/// </summary>
/// <param name="repository">The repository for accessing notebook data.</param>
/// <param name="cache">The memory cache for storing notebook sections.</param>
internal sealed class NotebookService(INotebookRepository repository, IMemoryCache cache) : INotebookService
{
    public async Task<IEnumerable<Section>> GetSectionsAsync(string userId, CancellationToken cancellationToken)
    {
        return await GetSectionsFromCacheAsync(userId, cancellationToken);
    }

    public async Task<IEnumerable<Section>> SearchSectionsAndPagesWithMatchesAsync(string userId, string searchText, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return [];
        }

        string text = RemoveDiacritics(searchText.Trim().ToLower());
        var sections = await GetSectionsFromCacheAsync(userId, cancellationToken);

        return sections.Where(section => RemoveDiacritics(section.Name.ToLower()).Contains(text)
                                        || section.Pages.Any(page => RemoveDiacritics(page.Title.ToLower()).Contains(text)));
    }

    public async Task CreateSectionAsync(string userId, Section? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.UserId = userId;
        entity.Pages.Add(new Page()
        {
            Title = "Nepojmenovaná stránka",
            CreatedAt = DateTimeOffset.UtcNow
        });

        await repository.CreateSectionAsync(entity, cancellationToken);

        cache.Remove(userId);
    }

    public async Task UpdateSectionAsync(string userId, Section? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.Name = !string.IsNullOrEmpty(entity.Name) ? entity.Name : "Nepojmenovaný oddíl";
        await repository.UpdateSectionAsync(entity, userId, cancellationToken);

        cache.Remove(userId);
    }


    public async Task DeleteSectionAsync(string userId, int id, CancellationToken cancellationToken)
    {
        await repository.DeleteSectionAsync(id, userId, cancellationToken);

        cache.Remove(userId);
    }

    public async Task AddPageAsync(string userId, Page? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.Title = !string.IsNullOrEmpty(entity.Title) ? entity.Title : "Nepojmenovaná stránka";
        entity.Content ??= string.Empty;
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.SizeInBytes = Encoding.UTF8.GetBytes(entity.Content).LongLength;

        await repository.AddPageAsync(entity, userId, cancellationToken);

        cache.Remove(userId);
    }

    public async Task UpdatePageAsync(string userId, Page? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.Title = !string.IsNullOrEmpty(entity.Title) ? entity.Title : "Nepojmenovaná stránka";
        entity.Content ??= string.Empty;
        entity.SizeInBytes = Encoding.UTF8.GetBytes(entity.Content).LongLength;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.UpdatePageAsync(entity, userId, cancellationToken);

        cache.Remove(userId);
    }

    public async Task DeletePageAsync(string userId, int id, CancellationToken cancellationToken)
    {
        await repository.DeletePageAsync(id, userId, cancellationToken);

        cache.Remove(userId);
    }

    public async Task<string> GetPageContentById(string userId, int id, CancellationToken cancellationToken)
    {
        return await repository.GetPageContentById(id, userId, cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves sections from the cache. 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable collection of sections.</returns>
    private async Task<IEnumerable<Section>> GetSectionsFromCacheAsync(string userId, CancellationToken cancellationToken)
    {
        //var user = GetAuthenticatedUser();

        return await cache.GetOrCreateAsync(userId, async entry =>
        {
            return await repository.GetSectionsAsync(userId, cancellationToken);
        }) ?? [];
    }

    /// <summary>
    /// Removes diacritics from the specified text.
    /// </summary>
    /// <param name="text">The text from which to remove diacritics.</param>
    /// <returns>The text without diacritics.</returns>
    private static string RemoveDiacritics(string text)
    {
        return new string(text.Normalize(NormalizationForm.FormD)
                              .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                              .ToArray()
        );
    }
}