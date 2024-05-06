using AutoMapper;
using Talabat.Core.Entities;
//using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
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

			
			CreateMap<Address, AddressDto>();
			//CreateMap<Address, AddressDto>().ReverseMap();
			CreateMap<Order,OrderToReturnDto>().ForMember(d=>d.DeliveryMethod,O=>O.MapFrom (s=>s.DeliveryMethod .ShortName ) ).
				ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));
			
			CreateMap<OrderItem, OrderItemDto>().
				ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
				.ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName)).
				   ForMember(d => d.PictureUrl , O => O.MapFrom(s => s.Product.PictureUrl )).
					ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());

		}

	}
}
