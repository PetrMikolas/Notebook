using Notebook.Exceptions;
using Notebook.Models;
using System.Security.Claims;

namespace Notebook.Services.User;

/// <summary>
/// Service responsible for managing user-related operations, implementing the <seealso cref="IUserService"/> interface.
/// </summary>
internal sealed class UserService(ILogger<UserService> logger) : IUserService
{    
    /// <summary>
    /// Gets or sets the current user.
    /// </summary>
    public CurrentUser CurrentUser { get; set; } = new CurrentUser(string.Empty, string.Empty, false);
	    
    public void FetchCurrentUser(ClaimsPrincipal user)
	{		
		try
		{
			var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (user.Identity is not null && user.Identity.Name is not null && userId is not null)
			{
				CurrentUser = new CurrentUser(userId, user.Identity.Name, user.Identity.IsAuthenticated);
			}
			else
			{
				throw new UserFetchException("Nepodařilo se načíst informace o uživateli. Uživatel není ověřen.");
			}
		}
		catch (UserFetchException)
		{			
			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Chyba při získávání informací o uživateli.");
			throw new UserFetchException("Neočekávaná chyba při získávání informací o uživateli.", ex);
		}
	}
}