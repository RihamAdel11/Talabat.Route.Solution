using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class ProductItemOrdered
	{
       private ProductItemOrdered()
        {
            
        }
        public ProductItemOrdered(int productId,string name,string picture)
        {
            productId = ProductId;
            name = ProductName;
              picture= PictureUrl;

            
        }
        public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
	}
}
