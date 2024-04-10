using Notebook.Models;
using System.Security.Claims;

namespace Notebook.Services.User;

public interface IUserService
{    
	void FetchCurrentUser(ClaimsPrincipal user);

	CurrentUser CurrentUser { get; }
}