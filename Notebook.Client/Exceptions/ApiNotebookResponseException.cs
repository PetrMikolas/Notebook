namespace Notebook.Client.Exceptions;

/// <summary>
/// Represents an exception thrown when there is an issue with the response from the notebook API.
/// </summary>
public class ApiNotebookResponseException : Exception
{
    public ApiNotebookResponseException() { }

    public ApiNotebookResponseException(string message) : base(message) { }

    public ApiNotebookResponseException(string message, Exception inner) : base(message, inner) { }
}