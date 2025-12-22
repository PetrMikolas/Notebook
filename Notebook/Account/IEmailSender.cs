using Notebook.Models;

namespace Notebook.Account;

internal interface IEmailSender
{
    Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink);
    Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink);
    Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode);
}