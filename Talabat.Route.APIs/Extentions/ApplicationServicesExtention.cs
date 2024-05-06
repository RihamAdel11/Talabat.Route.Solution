 using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.Route.APIs.Errors;

using Talabat.Core.Repositries.Contract;
using Talabat.Repositry;
using Talabat.Route.APIs.Helpers;
using Talabat.Core;


namespace Talabat.Route.APIs.Extentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection  AddAplicationServices( this IServiceCollection Services)
		{
			Services.AddScoped(typeof(IUnitOfWork ),typeof(UnitOfWork ));
			Services.AddScoped<IBasketRepositry, BasketRepositry>();
			Services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            Services.AddAutoMapper(typeof(MappingProfile).Assembly);
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
