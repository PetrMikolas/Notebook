using MimeKit.Text;
using System.Runtime.CompilerServices;

namespace Notebook.Services.Email;


/// <summary>
/// Service for sending emails. 
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an error message asynchronously.
    /// </summary>
    /// <param name="errorMessage">The error message to be sent.</param>
    /// <param name="cancellationToken">The cancellation token (optional). Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendErrorAsync(string errorMessage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an error message along with contextual information about the caller's source file and member name.
    /// </summary>
    /// <param name="errorMessage">The error message to be sent.</param>
    /// <param name="callerFilePath">Automatically supplied path of the source file containing the caller.</param>
    /// <param name="callerMemberName">Automatically supplied name of the calling method or property.</param>
    /// <param name="cancellationToken">The cancellation token (optional). Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendErrorWithContextAsync(string errorMessage, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="message">The message content of the email.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="address">The recipient email address.</param>
    /// <param name="name">The recipient name (optional). Defaults to an empty string.</param>
    /// <param name="textFormat">The format of the email content (optional). Defaults to <see cref="TextFormat.Plain"/>.</param>
    /// <param name="cancellationToken">The cancellation token (optional). Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string message, string subject, string address, string name = "", TextFormat textFormat = TextFormat.Plain, CancellationToken cancellationToken = default);
}