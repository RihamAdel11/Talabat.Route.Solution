 using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repositry.Data.Config.Order_Config
{
	internal class OrderConfigrations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
			builder.Property(order => order.Status).HasConversion(
				(ostatus) => ostatus.ToString(),
				(ostatus) =>(OrderStatus)Enum.Parse(typeof(OrderStatus), ostatus));
			//builder.HasOne(order => order.DeliveryMethod).WithOne(); 
			//builder.HasIndex ("DeliveryMethodId").IsUnique(true);
			builder.Property(order => order.SubTotal).HasColumnType("decimal(12,2)");
			builder.HasOne(order => order.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
			builder.HasMany(order => order.Items).WithOne().OnDelete(DeleteBehavior.Cascade);
		}
	}
}
