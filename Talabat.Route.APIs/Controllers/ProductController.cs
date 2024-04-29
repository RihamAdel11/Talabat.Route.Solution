using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositries.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProdectSpec;
using Talabat.Route.APIs.DTOs;
using Talabat.Route.APIs.Errors;

namespace Talabat.Route.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseAPIController
    {
        private readonly IGenericRepositry<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepositry<ProductCategory> _categoriesRepo;
		private readonly IGenericRepositry<ProductBrand> _brandRepo;

		public ProductController(IGenericRepositry<Product> productRepo, IGenericRepositry<ProductBrand> BrandRepo, IGenericRepositry<ProductCategory> CategoriesRepo
		, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _categoriesRepo = CategoriesRepo;
            _brandRepo = BrandRepo;
        }
        [HttpGet]
    
        public async Task<ActionResult<IEnumerable<ProductToReturn >>> GetProducts()
        {
            var spec = new ProductWithBrandCategory();
            var product = await _productRepo.GetAllAsyncWithSpec(spec);
            return Ok(_mapper.Map<IEnumerable < Product>,IEnumerable < ProductToReturn>>(product));

        }
        [ProducesResponseType(typeof(ProductToReturn), 200)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturn >>GetProduct(int id){

            var spec = new ProductWithBrandCategory();
            var product = await _productRepo.GetAsyncWithSpec(spec);

        if(product is null)
            {
                return NotFound();

            }
            return Ok(_mapper.Map<Product,ProductToReturn>(product));
                }
		[HttpGet("brands")]
		public async Task<ActionResult<IEnumerable <ProductBrand>>> GetBrands()
		{
			var brands = await _brandRepo.GetAllAsync();
			return Ok(brands);
		}
		[HttpGet("Categories")]
		public async Task<ActionResult<IEnumerable <ProductCategory>>> GetCategoris()
		{
			var Categories = await _brandRepo.GetAllAsync();
			return Ok(Categories);
		}

	}
}
