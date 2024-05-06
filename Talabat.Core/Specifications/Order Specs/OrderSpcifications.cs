using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specs
{
	public class OrderSpcifications:BaseSpecifications <Order>
	{

        public OrderSpcifications(string buyerEmail):base(O=>O.BuyerEmail ==buyerEmail )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O => O.OrderDate );
            
        }
        public OrderSpcifications(int orderId,string email):base(
            O=>O.Id==orderId&& O.BuyerEmail ==email )
        {
			Includes.Add(O => O.DeliveryMethod);
			Includes.Add(O => O.Items);

		}
    }
}
