using Microsoft.AspNetCore.Identity;
using MimeKit.Text;
using Notebook.Services.Email;
using Notebook.Models;

namespace Notebook.Account;

/// <summary>
/// Provides functionality for sending email notifications related to user account management.
/// </summary>
/// <param name="emailService"></param>
internal sealed class EmailSender(IEmailService emailService) : IEmailSender<AppUser>
{
    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        string message = $"Prosím potvrďte svůj e-mail <a href='{confirmationLink}'>kliknutím zde</a>.<br/><br/>Odesláno z aplikace Zápisník";

        try
        {
            await emailService.SendEmailAsync(message, "Potvrzení e-mailu", email, user.UserName ??= string.Empty, TextFormat.Html);            
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
    {
        string message = $"Prosím obnovte své heslo <a href='{resetLink}'>kliknutím zde</a>.<br/><br/>Odesláno z aplikace Zápisník";

        try
        {
            await emailService.SendEmailAsync(message, "Obnovení hesla", email, user.UserName ??= string.Empty, TextFormat.Html);            
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
    {
        string message = $"Obnovte své heslo pomocí následujícího kódu: {resetCode}<br/><br/>Odesláno z aplikace Zápisník";

        try
        {
            await emailService.SendEmailAsync(message, "Obnovení hesla", email, user.UserName ??= string.Empty, TextFormat.Html);            
        }
        catch (Exception)
        {
            throw;
        }
    }
}
