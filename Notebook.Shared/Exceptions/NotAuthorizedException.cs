namespace Notebook.Shared.Exceptions;

/// <summary>
/// Represents errors that occur when a user is not authorized to perform a specific action.
/// </summary>
public class NotAuthorizedException : Exception
{
    public NotAuthorizedException() { }

    public NotAuthorizedException(string message) : base(message) { }

    public NotAuthorizedException(string message, Exception inner) : base(message, inner) { }
}