using Microsoft.AspNetCore.Builder;

namespace Talabat.Route.APIs.Extentions
{
    public static class SwaggerServicesExtention
    {
        public static IServiceCollection AddSwagerServices(this IServiceCollection Services)
        {
           Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
            return Services;    
        }
        
        }
}
