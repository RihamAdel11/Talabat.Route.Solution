using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.Route.APIs.Extentions
{
	public static class UserManagerExtension
	{
		public static async Task<ApplicationUser?>FindUserWithAddressByEmail(this UserManager<ApplicationUser >userManager,ClaimsPrincipal User)
		{
			var email=User.FindFirstValue(ClaimTypes .Email);
			var user=await userManager .Users .Include (U=>U.Address ).FirstOrDefaultAsync(U=>U.NormalizedEmail == email.ToUpper ());
			return user;
		}
	}
}
