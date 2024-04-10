namespace Notebook.Exceptions;

/// <summary>
/// Represents an error that occurs when information about a user cannot be fetched.
/// </summary>
public class UserFetchException : Exception
{
	public UserFetchException() : base("Nepodařilo se načíst informace o uživateli") { }

	public UserFetchException(string message) : base(message) { }

	public UserFetchException(string message, Exception innerException) : base(message, innerException) { }
}