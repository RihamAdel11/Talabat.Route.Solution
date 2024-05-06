using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Talabat.Route.APIs.DTOs;
using Talabat.Route.APIs.Errors;

namespace Talabat.Route.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrdersController : BaseAPIController
	{
		private readonly IOrderServices _orderservices;
		private readonly IMapper _mapper;

		public OrdersController(IOrderServices orderservices, IMapper mapper)
		{
			_orderservices = orderservices;
			_mapper = mapper;
		}
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
			var order = await _orderservices.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, address, orderDto.DeliveryMethodId);
			if (order is null) return BadRequest(new APIResponse(400));
			return Ok(order);
		}
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser(string email)
		{
			var orders=await _orderservices .GetOrderForUserAsync(email);
			return Ok(orders);
		}
	}
}
