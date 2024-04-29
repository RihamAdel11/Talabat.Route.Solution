using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProdectSpec
{
   public class ProductWithBrandCategory:BaseSpecifications <Product>
    {


        public ProductWithBrandCategory(ProductSpecParams specparams) :base(p=>
        (string.IsNullOrEmpty (specparams .Search)||(p.Name.ToLower().Contains(specparams .Search))&&
        (!specparams.BrandId .HasValue ||p.BrandId== specparams.BrandId.Value)&&(!specparams.CategoryId .HasValue ||p.CategoryId==specparams.CategoryId .Value )))
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
            if(!string.IsNullOrEmpty(specparams.Sort)) {
                switch(specparams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price); break;
                    case "priceDesc":
                        AddOrderBy(p => p.Price); break;
                    default:
                        AddOrderBy (p=>p.Name ); break; 
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
            ApplyPagination((specparams .PageIndex -1)*specparams.pagesize ,specparams .pagesize );
        }
        public ProductWithBrandCategory(int id) : base(
    p => p.Id == id)
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }

    }
}
