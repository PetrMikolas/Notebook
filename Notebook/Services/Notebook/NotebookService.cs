using Microsoft.Extensions.Caching.Memory;
using Notebook.Helpers;
using Notebook.Models;
using Notebook.Repositories.Notebook;
using Notebook.Services.User;
using Notebook.Shared.Exceptions;
using System.Text;

namespace Notebook.Services.Notebook;

/// <summary>
/// Service for managing notebook sections and pages based on the <seealso cref="INotebookService"/> interface.
/// </summary>
/// <param name="repository">The repository for accessing notebook data.</param>
/// <param name="cache">The memory cache for storing notebook sections.</param>
/// <param name="userService">Service responsible for managing user-related operations.</param>
internal sealed class NotebookService(INotebookRepository repository, IMemoryCache cache, IUserService userService) : INotebookService
{    
	public async Task<List<Section>> GetSectionsAsync(CancellationToken cancellationToken)
    {        
		return await GetSectionsFromCacheAsync(cancellationToken);
    }
       
    public async Task<List<Section>> SearchSectionsAsync(string searchText, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return [];
        }

        string text = Helper.RemoveDiacritics(searchText.Trim().ToLower());
        var sections = await GetSectionsFromCacheAsync(cancellationToken);

        return sections.Where(section => Helper.RemoveDiacritics(section.Name.ToLower()).Contains(text)
                                        || section.Pages.Any(page => Helper.RemoveDiacritics(page.Title.ToLower()).Contains(text))).ToList();
    }
     
    public async Task CreateSectionAsync(Section? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
		var user = GetAuthenticatedUser();

		entity.UserId = user.Id;
        entity.Pages.Add(new Page()
        {
            Title = "Nepojmenovaná stránka",
            CreatedDate = DateTimeOffset.UtcNow
        });

        await repository.CreateSectionAsync(entity, cancellationToken);

        cache.Remove(user.Id);
    }
        
    public async Task UpdateSectionAsync(Section? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
		var user = GetAuthenticatedUser();

		entity.Name = !string.IsNullOrEmpty(entity.Name) ? entity.Name : "Nepojmenovaný oddíl";
        await repository.UpdateSectionAsync(entity, user.Id, cancellationToken);

        cache.Remove(user.Id);
    }

   
    public async Task DeleteSectionAsync(int id, CancellationToken cancellationToken)
    {
		var user = GetAuthenticatedUser();

		await repository.DeleteSectionAsync(id, user.Id, cancellationToken);

		cache.Remove(user.Id);
    }
       
    public async Task AddPageAsync(Page? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
		var user = GetAuthenticatedUser();

		entity.Title = !string.IsNullOrEmpty(entity.Title) ? entity.Title : "Nepojmenovaná stránka";
        entity.Content ??= string.Empty;
        entity.CreatedDate = DateTimeOffset.UtcNow;
        entity.SizeInBytes = Encoding.UTF8.GetBytes(entity.Content).LongLength;

        await repository.AddPageAsync(entity, user.Id, cancellationToken);

		cache.Remove(user.Id);
    }
      
    public async Task UpdatePageAsync(Page? entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
		var user = GetAuthenticatedUser();

		entity.Title = !string.IsNullOrEmpty(entity.Title) ? entity.Title : "Nepojmenovaná stránka";
        entity.Content ??= string.Empty;
        entity.SizeInBytes = Encoding.UTF8.GetBytes(entity.Content).LongLength;
        entity.ModifiedDate = DateTimeOffset.UtcNow;

        await repository.UpdatePageAsync(entity, user.Id, cancellationToken);

		cache.Remove(user.Id);
    }
       
    public async Task DeletePageAsync(int id, CancellationToken cancellationToken)
    {
		var user = GetAuthenticatedUser();

		await repository.DeletePageAsync(id, user.Id, cancellationToken);

		cache.Remove(user.Id);
    }
       
    public async Task<string> GetPageContentById(int id, CancellationToken cancellationToken)
    {
        var user = GetAuthenticatedUser();

		return await repository.GetPageContentById(id, user.Id, cancellationToken);
    }

    /// <summary>
    /// Gets the sections from cache asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of sections from cache.</returns>
    private async Task<List<Section>> GetSectionsFromCacheAsync(CancellationToken cancellationToken)
    {
		var user = GetAuthenticatedUser();

		return await cache.GetOrCreateAsync(user.Id, async entry =>
        {
            return await repository.GetSectionsAsync(user.Id, cancellationToken);

        }) ?? [];
    }

    /// <summary>
    /// Gets the authenticated user.
    /// </summary>
    /// <returns>The authenticated user.</returns>
    private CurrentUser GetAuthenticatedUser()
    {        
        if (!userService.CurrentUser.IsAuthenticated)
        {
            throw new NotAuthorizedException("Uživatel není ověřen.");
        }

        return userService.CurrentUser;
    }
}