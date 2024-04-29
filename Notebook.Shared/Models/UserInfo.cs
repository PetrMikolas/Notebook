namespace Notebook.Shared.Models;

/// <summary>
/// Represents information about a user.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Gets or sets the user's unique identifier.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public required string Email { get; set; }
}