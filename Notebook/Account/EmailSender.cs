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
/// <param name="httpContextAccessor">Provides access to the current HTTP context, used to construct URLs included in email messages.</param>
internal sealed class EmailSender(IEmailService emailService, ILogger<EmailSender> logger, IHttpContextAccessor httpContextAccessor) : IEmailSender<AppUser>
{
    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        ValidateEmailParameters(user, email, confirmationLink);        

        string message = $"Dobrý den,<br/><br/>" +
                         $"prosím potvrďte svou e-mailovou adresu kliknutím <a href='{confirmationLink}'>zde</a>.<br/><br/>" +
                         $"Po potvrzení e-mailové adresy se budete moci přihlásit do svého účtu v aplikaci Zápisník pomocí ověřeného e-mailu.<br/><br/>" +
                         $"Odesláno z aplikace <a href='{GetBaseUrl()}'>Zápisník</a>";

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
                         $"Odesláno z aplikace <a href='{GetBaseUrl()}'>Zápisník</a>";

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

    private string GetBaseUrl()
    {
        var request = httpContextAccessor.HttpContext?.Request;

        return $"{request?.Scheme}://{request?.Host}";
    }
}
