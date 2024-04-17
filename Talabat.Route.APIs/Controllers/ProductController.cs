using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositries.Contract;

namespace Talabat.Route.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepositry<Product> _productRepo;

        public ProductController(IGenericRepositry<Product>productRepo)
        {
           _productRepo = productRepo;
        }
    }
}
