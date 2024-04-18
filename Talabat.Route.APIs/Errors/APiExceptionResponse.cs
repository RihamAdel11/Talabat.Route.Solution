using System.Security.Cryptography.X509Certificates;
using Talabat.Route.APIs.Errors;
namespace Talabat.Route.APIs.Errors
{
    public class APiExceptionResponse:APIResponse
    {
        public string? Details { get; set; }
        public APiExceptionResponse(int statusCode,string?message=null,string? details=null):
            base(statusCode, message)
        {
            Details=details;
        }
    }
}
