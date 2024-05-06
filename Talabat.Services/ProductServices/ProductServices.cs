using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProdectSpec;

namespace Talabat.Services.ProductServices
{
	public class ProductServices : IProductServices
	{
		private readonly IUnitOfWork _unitofWork;

		public ProductServices(IUnitOfWork unitofWork)
        {
			_unitofWork = unitofWork;
		}
		public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
		=> await _unitofWork.Repositry<ProductBrand>().GetAllAsync();

		public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
		=> await _unitofWork.Repositry<ProductCategory>().GetAllAsync();

		public async Task<int> GetCountAsync(ProductSpecParams specparams)
		{
			var countspec = new ProductWithFilterationForCount(specparams);
			var count = await _unitofWork .Repositry <Product >().GetCountAsync(countspec);
			return count;
		}

		public async Task<Product> GetProductAsync(int id)
		{
			var spec = new ProductWithBrandCategory(id);
			var product = await _unitofWork .Repositry <Product >().GetAsyncWithSpec(spec);
			return product;
		}

		public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specparams)
		{
			var spec = new ProductWithBrandCategory(specparams);
			var product = await _unitofWork .Repositry <Product >().GetAllAsyncWithSpec(spec);
			return product;
		}
	}
}
