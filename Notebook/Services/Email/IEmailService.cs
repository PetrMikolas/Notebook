using MimeKit.Text;

namespace Notebook.Services.Email;

/// <summary>
/// Service for sending emails. 
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an error message asynchronously.
    /// </summary>
    /// <param name="errorMessage">The error message to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendErrorAsync(string errorMessage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an error message asynchronously, including information about the type and method where the error occurred.
    /// </summary>
    /// <param name="errorMessage">The error message to send.</param>
    /// <param name="typeClass">The type of the class where the error occurred.</param>
    /// <param name="nameMethod">The name of the method where the error occurred.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendErrorAsync(string errorMessage, Type typeClass, string nameMethod, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an object serialized to JSON asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The type of the object to send.</typeparam>
    /// <param name="value">The object to send.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendObjectAsync<TValue>(TValue value, string subject, CancellationToken cancellationToken = default) where TValue : class;

    /// <summary>
    /// Sends email asynchronously.
    /// </summary>
    /// <param name="message">The content of the email.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="address">The recipient's email address.</param>
    /// <param name="name">The recipient's name.</param>
    /// <param name="textFormat">The format of the email content (Plain or HTML).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendEmailAsync(string message, string subject, string address, string name = "", TextFormat textFormat = TextFormat.Plain, CancellationToken cancellationToken = default);
}