using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositries.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services.OrderServices
{
	public class OrderServices : IOrderServices
	{
		private readonly IBasketRepositry _basketRepo;
		private readonly IUnitOfWork _unitofwork;
		private readonly IPaymentServices _paymentServices;

		public OrderServices(IBasketRepositry basketRepo,IUnitOfWork unitofwork,IPaymentServices paymentServices
			)
        {
			_basketRepo = basketRepo;
             _unitofwork = unitofwork;
			_paymentServices = paymentServices;
		}
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, Address ShippingAddress, int deliveryMethodId)
		{
			var basket = await _basketRepo.GetBasketAsync(BasketId);
			var orderItems = new List<OrderItem>();
			if(basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items )
				{ var product = await _unitofwork.Repositry <Product>().GetAsync(item.Id);
					var ProductOrdered = new ProductItemOrdered(product.Id,product .Name , product.PictureUrl);
					var orderItem = new OrderItem(ProductOrdered, product.Price, item.Quntity);

				}
			}

			var Subtotal= orderItems.Sum(item => item.Price*item.Quntity);

			var deliveryMethods = await _unitofwork.Repositry <DeliveryMethod >().GetAsync(deliveryMethodId);
			var orderRepo = _unitofwork.Repositry<Order>();
			var spec = new OrderSpcifications(basket?.PaymentIntentId);
			var existingOrder = await orderRepo.GetAsyncWithSpec(spec);
			if (existingOrder != null)
			{
				orderRepo.Delete(existingOrder);
				await _paymentServices.CreateOrUpdatePaymentIntent(BasketId);
			}

			
			
			var order = new Order(
				buyerEmail: BuyerEmail,
			  shippingAddress: ShippingAddress,
			   deliverymethod: deliveryMethods,
				Item: orderItems,
				subtotal: Subtotal,
				paymentIntentId:basket?.PaymentIntentId??""

				);


			_unitofwork.Repositry <Order>().Add(order );
			var result=await _unitofwork.CompleteAsync();
			if (result <= 0) return null;
			return order;



		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
		=> await _unitofwork.Repositry<DeliveryMethod>().GetAllAsync();

		public Task<Order?> GetOrderByIdForUserAsync( int OrderId, string BuyerEmail)
		{
			var orderRepo=_unitofwork .Repositry <Order>();	
			var orderSpec=new OrderSpcifications (OrderId, BuyerEmail);
			var order=orderRepo .GetAsyncWithSpec (orderSpec);
			return order;
		}

		public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
		{
			var ordersRepo = _unitofwork.Repositry<Order>();
			var spec = new OrderSpcifications(buyerEmail);
			var orders=await ordersRepo .GetAllAsyncWithSpec (spec);
			return orders;

		}
	}
}
