using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositries.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProdectSpec;

namespace Talabat.Route.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseAPIController
    {
        private readonly IGenericRepositry<Product> _productRepo;

        public ProductController(IGenericRepositry<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var spec = new ProductWithBrandCategory();
            var product = await _productRepo.GetAllAsyncWithSpec(spec);
            return Ok(product);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>>GetProduct(int id){

            var spec = new ProductWithBrandCategory();
            var product = await _productRepo.GetAsyncWithSpec(spec);

        if(product is null)
            {
                return NotFound();

            }
            return Ok(product);
                }
    }
}
