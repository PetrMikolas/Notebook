namespace Notebook.Models;

/// <summary>
/// Represents the current user's information.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Name">The name of the user.</param>
/// <param name="IsAuthenticated">Indicates whether the user is authenticated.</param>
public record CurrentUser(string Id, string Name, bool IsAuthenticated);