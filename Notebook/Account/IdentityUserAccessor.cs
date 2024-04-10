using Microsoft.AspNetCore.Identity;
using Notebook.Models;

namespace Notebook.Account;

internal sealed class IdentityUserAccessor(UserManager<AppUser> userManager, IdentityRedirectManager redirectManager)
{
    public async Task<AppUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Nelze naèíst uživatele s ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}