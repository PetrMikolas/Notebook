using Notebook.Models;
using System.Security.Claims;

namespace Notebook.Services.User;

/// <summary>
/// Interface for managing user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets the current user.
    /// </summary>
    CurrentUser CurrentUser { get; }

    /// <summary>
    /// Fetches the current user based on the claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the current user.</param>
    void FetchCurrentUser(ClaimsPrincipal user);
}