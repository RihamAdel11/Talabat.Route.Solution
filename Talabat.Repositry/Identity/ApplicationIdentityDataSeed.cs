using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repositry.Identity
{
	public static class ApplicationIdentityDataSeed
	{
		public static async Task SeedUserAsync(UserManager<ApplicationUser> userManger)
		{
			if (!userManger.Users.Any())

			{
				var user = new ApplicationUser()
				{
					DisplayName = "Riham Adel",
					Email = "rihammohareb6@gmail.com",
					UserName = "RihamMohareb",
					PhoneNumber = "01210548629"
				};
				await userManger.CreateAsync(user,"Pa$$w0rd");
				}

			
		}
	}
}
