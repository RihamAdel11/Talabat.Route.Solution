using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Route.APIs.Errors;
using Talabat.Route.APIs.Controllers;


namespace Talabat.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings (IgnoreApi =true)]
    public class ErrorsController :BaseAPIController 
    {
        public ActionResult Error(int code)
        {
            if (code == 400)
                return BadRequest(new APIResponse(400));
            else if (code == 404)
                return NotFound(new APIResponse(code));
            else if (code == 401)
                return Unauthorized(new APIResponse(401));
            else return BadRequest(new APIResponse(code));

        }
    }
}
