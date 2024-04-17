using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }//forigen key column=>ProductBrand
        public int CategoryId { get; set; }//forigen key column=>ProductCategory
        public ProductBrand Brand { get; set; }//Nav Prop [one]
        public ProductCategory Category { get; set; }//Nav Prop [one]
        
    }
}
