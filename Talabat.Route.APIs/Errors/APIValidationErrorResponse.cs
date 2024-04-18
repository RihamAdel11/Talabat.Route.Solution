using Talabat.Route.APIs.Errors;


namespace Talabat.Route.APIs.Errors
{
    public class APIValidationErrorResponse:APIResponse 
    {
        public IEnumerable <string>Errors { get; set; }
        public APIValidationErrorResponse():base(400)
        {
            Errors = new List<string>();
            
        }
    }
}
