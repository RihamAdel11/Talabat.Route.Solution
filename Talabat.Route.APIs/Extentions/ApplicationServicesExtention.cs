using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.Route.APIs.Errors;

using Talabat.Core.Repositries.Contract;
using Talabat.Repositry;
using Talabat.Route.APIs.Helpers;


namespace Talabat.Route.APIs.Extentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection  AddAplicationServices( this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0).
                    SelectMany(p => p.Value.Errors).Select(E => E.ErrorMessage).ToList();
                    var response = new APIValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return Services;
        }
    }
}
