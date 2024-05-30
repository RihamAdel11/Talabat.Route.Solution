using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Route.APIs.Errors;

namespace Talabat.Route.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize ]
	public class PaymentController :BaseAPIController 
	{
		private readonly IPaymentServices _paymentServices;

		public PaymentController(IPaymentServices paymentServices)
        {_paymentServices = paymentServices;
		}
		[HttpGet("{basketid}") ]
		public async Task<ActionResult <CustomerBasket >>CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null) return BadRequest(new APIResponse(400, "An Error With your Basket"));
			return Ok(basket);
		}

    }
}
