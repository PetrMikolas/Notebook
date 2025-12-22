using Microsoft.AspNetCore.Identity;
using MimeKit.Text;
using Notebook.Models;
using Notebook.Services.Email;

namespace Notebook.Account;

/// <summary>
/// Provides functionality for sending email notifications related to user account management.
/// </summary> 
/// <param name="emailService">The email service used to send email messages to users.</param>
/// <param name="logger">The logger used to record errors.</param>
/// <param name="configuration">The application configuration.</param>
internal sealed class EmailSender(IEmailService emailService, ILogger<EmailSender> logger, IConfiguration configuration) : IEmailSender<AppUser>
{
    private readonly string baseUrl = configuration["BaseUrl"]
        ?? throw new InvalidOperationException("Configuration value 'BaseUrl' is not set.");

    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        ValidateEmailParameters(user, email, confirmationLink);        

        string message = $"Dobrý den,<br/><br/>" +
                         $"prosím potvrďte svou e-mailovou adresu kliknutím <a href='{confirmationLink}'>zde</a>.<br/><br/>" +
                         $"Po potvrzení e-mailové adresy se budete moci přihlásit do svého účtu v aplikaci Zápisník pomocí ověřeného e-mailu.<br/><br/>" +
                         $"Odesláno z aplikace <a href='{baseUrl}'>Zápisník</a>";

        try
        {
            await emailService.SendEmailAsync(message, "Potvrzení e-mailu", email, user.UserName ??= string.Empty, TextFormat.Html);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending confirmation link email to {email}", email);
            throw;
        }
    }

    public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
    {
        ValidateEmailParameters(user, email, resetLink);

        string message = $"Dobrý den,<br/><br/>" +
                         $"pro obnovení hesla prosím klikněte <a href='{resetLink}'>zde</a>.<br/><br/>" +
                         $"Odesláno z aplikace <a href='{baseUrl}'>Zápisník</a>";

        try
        {
            await emailService.SendEmailAsync(message, "Obnovení hesla", email, user.UserName ??= string.Empty, TextFormat.Html);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending password reset link email to {email}", email);
            throw;
        }
    }

    public async Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    private static void ValidateEmailParameters(AppUser user, string email, string link)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrWhiteSpace(link, nameof(link));
    }    
}
