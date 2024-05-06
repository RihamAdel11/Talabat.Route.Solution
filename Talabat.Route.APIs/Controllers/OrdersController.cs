﻿using AutoMapper;
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
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
			var order = await _orderservices.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, address, orderDto.DeliveryMethodId);
			if (order is null) return BadRequest(new APIResponse(400));
			return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
		}
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser(string email)
		{
			var orders = await _orderservices.GetOrderForUserAsync(email);
			return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
		}
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id, string email)
		{
			var order = await _orderservices.GetOrderByIdForUserAsync(id, email);
			if (order is null) return NotFound(new APIResponse(404));
			return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
		}
		[Authorize]
		[HttpGet("deliveryMethods")]
		public async Task<ActionResult<IReadOnlyList <DeliveryMethod >>> GetDeliveryMethods()
		{
			var deliveryMethod = await _orderservices.GetDeliveryMethodAsync();
			return Ok(deliveryMethod);
		}
	}
}