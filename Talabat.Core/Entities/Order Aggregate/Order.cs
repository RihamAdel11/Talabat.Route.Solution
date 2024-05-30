using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Order:BaseEntity 
	{
		private readonly DeliveryMethod? deliverymethod;

		private Order()
        {
            
        }
        public Order(string buyerEmail,Address shippingAddress,DeliveryMethod ?deliverymethod, ICollection<OrderItem> Item,
			decimal subtotal,string paymentIntentId)
        {
			buyerEmail = BuyerEmail;
			shippingAddress = ShippingAddress;
		    deliverymethod = DeliveryMethod;
			Item = Items;

			subtotal = SubTotal;
			PaymentIntendId = paymentIntentId;


		}
		public string BuyerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public Address ShippingAddress { get; set; } = null!;
		//public int DeliveryMethodId { get; set; }
		
		public DeliveryMethod? DeliveryMethod { get; set; } = null!;
		public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
		//[NotMapped ]
		public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntendId { get; set; }








    }
}
