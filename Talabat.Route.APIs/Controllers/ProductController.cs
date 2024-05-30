using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositries.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProdectSpec;
using Talabat.Route.APIs.DTOs;
using Talabat.Route.APIs.Errors;
using Talabat.Route.APIs.Helpers;

namespace Talabat.Route.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseAPIController
    {
        //      private readonly IGenericRepositry<Product> _productRepo;
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;
  //      private readonly IGenericRepositry<ProductCategory> _categoriesRepo;
		//private readonly IGenericRepositry<ProductBrand> _brandRepo;

		public ProductController(IProductServices productServices
		, IMapper mapper)
        {
           
			_productServices = productServices;
			_mapper = mapper;
        
        }
        [HttpGet]
        [Authorize]
		[CachedAttribute(600)]
		public async Task<ActionResult<Pagination <ProductToReturn >>> GetProducts([FromQuery] ProductSpecParams specparams)
        {
         
            var product = await _productServices .GetProductsAsync(specparams);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturn>>(product);
			
			var count = await _productServices.GetCountAsync (specparams );
			return Ok(new Pagination<ProductToReturn>(specparams.PageIndex , specparams.pagesize , count, data));

        }
        [ProducesResponseType(typeof(ProductToReturn), 200)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturn >>GetProduct(int id)
		{

            var product = await _productServices .GetProductAsync(id);

        if(product is null)
            {
                return NotFound();

            }
            return Ok(_mapper.Map<Product,ProductToReturn>(product));
                }
		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _productServices .GetBrandsAsync();
			return Ok(brands);
		}
		[HttpGet("Categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategoris()
		{
			var Categories = await _productServices .GetCategoriesAsync();
			return Ok(Categories);
		}

	}
}
