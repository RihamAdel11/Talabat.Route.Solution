using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
 public interface IOrderServices
	{
		Task<Order>CreateOrderAsync(string BuyerEmail,string BasketId,Address ShippingAddress,int deliveryMethodId);
		Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail);
		Task<Order> GetOrderByIdAsync(string BuyerEmail, int OrderId);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();


	}
}
