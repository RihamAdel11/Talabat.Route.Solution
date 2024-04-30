using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Route.APIs.DTOs;
using Talabat.Route.APIs.Errors;
using Talabat.Route.APIs.Extentions;

namespace Talabat.Route.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IAuthServices _authServices;
		private readonly IMapper _mapper;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAuthServices authServices, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_authServices = authServices;
			_mapper = mapper;
		}
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null) return Unauthorized(new APIResponse(401, "Invalid Login"));
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded) return Unauthorized(new APIResponse(401, "Invalid Login"));

			return Ok(new UserDto()

			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authServices.CreateTokenAsync(user, _userManager)
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
				Token = await _authServices.CreateTokenAsync(user, _userManager)
			});
		}
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
			var user = await _userManager.FindByEmailAsync(email);
			return Ok(new UserDto()
			{
				DisplayName = user?.DisplayName ?? string.Empty,
				Email = user?.Email ?? string.Empty,
				Token = await _authServices.CreateTokenAsync(user, _userManager)
			});
		}
		[Authorize]
		[HttpGet("Address")]
		public async Task<ActionResult<AddressDto>> GetUserAddress()
		{
			var user = await _userManager.FindUserWithAddressByEmail(User);
			return Ok(_mapper.Map<AddressDto>(user.Address));

		}
		[Authorize]
		[HttpGet("Address")]
		public async Task<ActionResult<Address>> UpdateUserAddress(AddressDto address)
		{
			var updatAddres = _mapper.Map<Address>(address);
			var user = await _userManager.FindUserWithAddressByEmail(User);
			updatAddres.Id = user.Address.Id;
			user.Address = updatAddres;
			var res = await _userManager.UpdateAsync(user);
			if (!res.Succeeded)
			{
				return BadRequest(new APIValidationErrorResponse()
				{
					Errors = res.Errors.Select(E => E.Description)
				});

			}
			return Ok(address);

		}
	}
}

