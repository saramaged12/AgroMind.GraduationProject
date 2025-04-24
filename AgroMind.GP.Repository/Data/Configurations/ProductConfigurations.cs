using AgroMind.GP.Core.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroMind.GP.Repository.Data.Configurations
{
	public class ProductConfigurations : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasOne(p => p.Brand) //(Brand Refer to the Name of Navigation Property that in Product)
				.WithMany()
				.HasForeignKey(p => p.BrandId)  // if i Changed The Name of Foreign Key
				//.OnDelete(DeleteBehavior.SetNull); // if Brand is deleted Not Delete Product and set The Foreign Key Null
			     .OnDelete(DeleteBehavior.Restrict); 
			builder.HasOne(C => C.Category)
				.WithMany()
				.HasForeignKey(C => C.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(P => P.Supplier)
				.WithMany(S => S.Products)
				.HasForeignKey(p => p.SupplierId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Property(p=>p.Price)
				.HasColumnType("decimal(18,2)"); // to avoid the error of decimal to double
		}
	}
}
