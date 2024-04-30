using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Talabat.Core.Entities.Identity;
using Talabat.Route.APIs.DTOs;
using Talabat.Route.APIs.Errors;

namespace Talabat.Route.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return Unauthorized(new APIResponse(401, "Invalid Login"));


			}
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded)
			{
				return Unauthorized(new APIResponse(401, "Invalid Login"));
			}
			return Ok(new UserDto()

			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = "this will be token"
			});


		}
		[HttpPost("Register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var user = new ApplicationUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split("@")[0],
				PhoneNumber = model.phone
			};
			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				return BadRequest(new APIValidationErrorResponse()
				{
					Errors = result.Errors.Select(E => E.Description)
				});

			}
			return Ok(new UserDto()

			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = "this will be token"
			});
		}
	}
}

