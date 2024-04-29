using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositries.Contract;
using Talabat.Route.APIs.DTOs;
using Talabat.Route.APIs.Errors;

namespace Talabat.Route.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketController : BaseAPIController 
	{
		private readonly IBasketRepositry _basketRepo;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepositry basketRepo, IMapper mapper)
		{
			_basketRepo = basketRepo;
			_mapper = mapper;
		}
		[HttpGet]
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
		{
			var basket = await _basketRepo.GetBasketAsync(id);
			return Ok(basket ?? new CustomerBasket(id));
		}
		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
		{
			var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
			var createdOrUpdated = await _basketRepo.UpdateBasketAsync(mappedBasket);
			if (createdOrUpdated is null) return BadRequest(new APIResponse(400));
			return Ok(createdOrUpdated);
		}
		[HttpDelete]
		public async Task DeletBasket(string id)
		{
			await _basketRepo.DeleteBasketAsync(id);
		}
	}
}
