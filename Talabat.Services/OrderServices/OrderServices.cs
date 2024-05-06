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

namespace Talabat.Services.OrderServices
{
	public class OrderServices : IOrderServices
	{
		private readonly IBasketRepositry _basketRepo;
		private readonly IUnitOfWork _unitofwork;
		

		public OrderServices(IBasketRepositry basketRepo,IUnitOfWork unitofwork
			)
        {
			_basketRepo = basketRepo;
             _unitofwork = unitofwork;
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
			var order = new Order(
				buyerEmail: BuyerEmail,
			  shippingAddress: ShippingAddress,
			   deliverymethod: deliveryMethods,
				Item: orderItems,
				subtotal: Subtotal

				);


			_unitofwork.Repositry <Order>().Add(order );
			var result=await _unitofwork.CompleteAsync();
			if (result <= 0) return null;
			return order;



		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdAsync(string BuyerEmail, int OrderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
