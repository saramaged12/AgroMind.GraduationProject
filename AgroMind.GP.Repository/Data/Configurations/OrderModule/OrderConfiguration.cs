using AgroMind.GP.Core.Entities.Orders;
using AgroMind.GP.Core.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Configurations.OrderModule
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
			builder.Property(o => o.Status)
				.HasConversion
				(
				(orderStatus)=> orderStatus.ToString(), //String in DB
				(orderStatus)=> (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus) //When reading from DB
				);
			builder.Property(o => o.Subtotal)
				.HasColumnType("decimal(8,2)");

			builder.HasOne(o => o.DeliveryMethod)
				.WithMany()
				.HasForeignKey(o => o.DeliveryMethodId)
				.OnDelete(DeleteBehavior.SetNull);
			builder.HasMany(o => o.OrderItems)
				.WithOne()
				.OnDelete(DeleteBehavior.Cascade); // when remove Order Remove OrderItems
		}
	}
}
