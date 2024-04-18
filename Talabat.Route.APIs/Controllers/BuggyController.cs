using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repositry.Data;
using Talabat.Route.APIs.Errors;

namespace Talabat.Route.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseAPIController 
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet("NotFound")]
        public IActionResult GetNotFoundRequest()
        {
            var product = _dbcontext.Products.Find(100);
            if (product == null)
            {
                return NotFound(new APIResponse(404));
            }
            return Ok(product);
        }
        [HttpGet("ServerError")]
        public IActionResult GetServerErrorRequest()
        {
            var product = _dbcontext.Products.Find(100);

            var prodectToReurn = product.ToString();

            return Ok(prodectToReurn);
        }
        [HttpGet("BadRequest")]
        public IActionResult GetbadRequestRequest()
        {


            return BadRequest(new APIResponse(400));
        }
        [HttpGet("BadRequest/{id}")]//Validation Error
        public IActionResult GetbadRequestRequest(int id)
        {


            return Ok();
        }
        [HttpGet("UnAuthorixedError")]
        public IActionResult GetUnAuthorixedErrorRequest()
        {


            return Unauthorized(new APIResponse(401));
        }
    }
}
