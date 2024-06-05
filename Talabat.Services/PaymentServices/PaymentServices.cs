using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositries.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services.PaymentServices
{
	public class PaymentServices : IPaymentServices
	{
		private readonly IConfiguration _configration;
		private readonly IBasketRepositry _basketRepo;
		private readonly IUnitOfWork _unitofWork;

		public PaymentServices(IConfiguration configration, IBasketRepositry basketRepo, IUnitOfWork unitofWork)
		{
			_configration = configration;
			_basketRepo = basketRepo;
			_unitofWork = unitofWork;
		}
		public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
		{
			StripeConfiguration.ApiKey = _configration["StripeSettings:Secretkey"];
			var basket = await _basketRepo.GetBasketAsync(basketId);
			if (basket is null) return null;
			var shippingPrice = 0M;
			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod = await _unitofWork.Repositry<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
				shippingPrice = deliveryMethod.Cost;
				basket.ShippingPrice = shippingPrice;
			}
			if (basket.Items?.Count > 0)
			{
				var productrepo = _unitofWork.Repositry<Product>();
				foreach (var item in basket.Items)
				{
					var product = await productrepo.GetAsync(item.Id);
					if (item.Price != product.Price)
						item.Price = product.Price;


				}
			}
			PaymentIntent paymentIntent;
			PaymentIntentService paymentIntentService = new PaymentIntentService();
			if (string.IsNullOrEmpty(basket.PaymentIntentId))
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quntity) + (long)shippingPrice * 100,
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }
				};
				paymentIntent = await paymentIntentService.CreateAsync(options);
			    basket.PaymentIntentId =paymentIntent .Id;
				basket.ClientSecret = paymentIntent .ClientSecret;
			
			}
			else
			{
				var Options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quntity) + (long)shippingPrice * 100
				};
				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, Options);

			}
			await _basketRepo.UpdateBasketAsync(basket);
			return basket;
		}
		public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
		{
			var orderRepo = _unitofWork.Repositry <Order>();
			var spec = new OrderWithPaymentIntentSpecifications (paymentIntentId);

			var order = await orderRepo.GetAsyncWithSpec(spec);

			if (order is null) return null;

			if (isPaid)
				order.Status = OrderStatus.PaymentReceived;
			else
				order.Status = OrderStatus.PaymentFailed;

			orderRepo.Update(order);

			await _unitofWork.CompleteAsync();

			return order;
		}


	}
}
