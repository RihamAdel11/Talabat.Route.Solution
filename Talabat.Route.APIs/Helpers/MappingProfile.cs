using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Route.APIs.DTOs;

namespace Talabat.Route.APIs.Helpers
{
    public class MappingProfile:Profile  
    {
        public MappingProfile()
        {
			CreateMap<Product, ProductToReturn>().
			   ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name)).
			   ForMember(B => B.Category, o => o.MapFrom(s => s.Category.Name)).
			   ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrl>());
			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();
			CreateMap<Address, AddressDto>().ReverseMap ();


		}

    }
}
