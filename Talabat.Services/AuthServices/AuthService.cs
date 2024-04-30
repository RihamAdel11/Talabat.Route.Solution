using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Services.AuthServices
{
	public class AuthService : IAuthServices
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public async Task<string> CreateTokenAsync(ApplicationUser User,UserManager <ApplicationUser >userManager)
		{ 
			var authClaims = new List<Claim>() {
		   new Claim(ClaimTypes .Name,User.DisplayName ),
		new Claim(ClaimTypes .Email,User.Email) };

			var userRole = await userManager.GetRolesAsync(User);
			foreach(var role in userRole)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			}
			var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));
			var token = new JwtSecurityToken(

				audience: _configuration["JWT:ValidAudience"],
				issuer : _configuration["JWT:ValidIssuser"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"] ?? "0")),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature));
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
