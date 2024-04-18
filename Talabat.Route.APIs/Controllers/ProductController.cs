using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositries.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProdectSpec;
using Talabat.Route.APIs.DTOs;

namespace Talabat.Route.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseAPIController
    {
        private readonly IGenericRepositry<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepositry<Product> productRepo,IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        [HttpGet]
    
        public async Task<ActionResult<IEnumerable<ProductToReturn >>> GetProducts()
        {
            var spec = new ProductWithBrandCategory();
            var product = await _productRepo.GetAllAsyncWithSpec(spec);
            return Ok(_mapper.Map<IEnumerable < Product>,IEnumerable < ProductToReturn>>(product));

        }
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
    }
}
