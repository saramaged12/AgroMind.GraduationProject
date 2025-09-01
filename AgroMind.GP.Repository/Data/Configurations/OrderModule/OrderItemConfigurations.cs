using AgroMind.GP.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Configurations.OrderModule
{
	public class OrderItemsConfigurations : IEntityTypeConfiguration<OrderItems>
	{
		public void Configure(EntityTypeBuilder<OrderItems> builder)
		{
			builder.OwnsOne(items=>items.Product,Product=>Product.WithOwner());
			builder.Property(item => item.Price)
				.HasColumnType("decimal(8,2)");
		}
	}
}
