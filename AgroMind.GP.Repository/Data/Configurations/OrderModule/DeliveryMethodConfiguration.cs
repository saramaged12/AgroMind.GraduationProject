using AgroMind.GP.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Configurations.OrderModule
{
	public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DeliveryMethod> builder)
		{
			builder.Property(deliveryMethod => deliveryMethod.Cost)
				.HasColumnType("decimal(8,2)");
		}
	}
}
