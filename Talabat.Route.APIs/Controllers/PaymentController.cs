using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Talabat.Route.APIs.Errors;
using Talabat.Services.PaymentServices;

namespace Talabat.Route.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize ]
	public class PaymentController :BaseAPIController 
	{
		const string WhSecret = "whsec_d293457753f93948a435fabe93117915ff5708303b23f452c32c0e27066e9fb3";

		private readonly IPaymentServices _paymentService;
		private readonly ILogger<PaymentController> _logger;

		public PaymentController(IPaymentServices paymentService, ILogger<PaymentController> logger)
		{
			_paymentService = paymentService;
			_logger = logger;
		}



		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		[HttpPost("{basketid}")] // Get : /api/Payment/{basketid}
		public async Task<ActionResult <CustomerBasket >>CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null) return BadRequest(new APIResponse(400, "An Error With your Basket"));
			return Ok(basket);
		}
		[HttpPost ("wwbhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			var stripeEvent = EventUtility.ConstructEvent(json,
				Request.Headers["Stripe-Signature"], WhSecret,300,false);

			var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

			Order order;


			switch (stripeEvent.Type)
			{
				case Events.PaymentIntentSucceeded:
					order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);

					_logger.LogInformation("Order Is Succeeded {0}", order?.PaymentIntendId );
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
					break;
				case Events.PaymentIntentPaymentFailed:
					order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);

					_logger.LogInformation("Order Is Failed {0}", order?.PaymentIntendId );
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

					break;
			}


			return Ok();
		}

	}
}
