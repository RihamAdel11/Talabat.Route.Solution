using AutoMapper;
using AutoMapper.Execution;
using Talabat.Core.Entities;
using Talabat.Route.APIs.DTOs;

namespace Talabat.Route.APIs.Helpers
{
    public class ProductPictureUrl:IValueResolver <Product, ProductToReturn, string>
    {
        public ProductPictureUrl(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration _Configuration { get; }

        public string Resolve(Product source, ProductToReturn destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_Configuration["ApiUrl"]}/{source.PictureUrl}";
            }
            return string.Empty;
            ;
        }
    }
}
