using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.ProdectSpec;

namespace Talabat.Core.Services.Contract
{
	public interface IProductServices
	{
		Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specparams);
		Task<Product> GetProductAsync(int id);
		Task<int> GetCountAsync(ProductSpecParams specparams);
		Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
		Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
	}
}
