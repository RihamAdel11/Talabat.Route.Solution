
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Route.APIs.DTOs;

namespace Talabat.Route.APIs.Helpers
{
	public class OrderItemPictureUrlResolver: IValueResolver<OrderItem, OrderItemDto , string>
	{
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
			_Configuration = configuration;
		}
		public IConfiguration _Configuration { get; }
		

	

		public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return $"{_Configuration["ApiUrl"]}/{source.Product.PictureUrl}";
			}
			return string.Empty;
			;
		}
	}
}
