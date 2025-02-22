using AgroMind.GP.Core.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Configurations
{
	public class ProductConfigurations : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasOne<Brand>(p => p.Brand) //(Brand Refer to the Name of Navigation Property that in Product)
				.WithMany()
				.HasForeignKey(p=>p.BrandId)  // if i Changed The Name of Foreign Key
				.OnDelete(DeleteBehavior.SetNull); // if Brand is deleted Not Delete Product and set The Foreign Key Null
		         
		    builder.HasOne(C=>C.Category)
				.WithMany()
				.HasForeignKey(C=>C.CategoryId)
				.OnDelete(DeleteBehavior.SetNull);
			builder.HasOne(S => S.Supplier)
				.WithMany()
				.HasForeignKey(S => S.SupplierId)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}
