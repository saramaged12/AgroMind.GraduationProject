using AgroMind.GP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroMind.GP.Repository.Data.Configurations
{
	public class CropConfiguration : IEntityTypeConfiguration<Crop>
	{
		public void Configure(EntityTypeBuilder<Crop> builder)
		{
		
			builder.HasOne(c => c.Land)
				.WithMany(l => l.Crops)
				.HasForeignKey(c => c.LandId)
				.OnDelete(DeleteBehavior.Restrict);
			// .OnDelete(DeleteBehavior.Cascade);  // If Land is deleted, delete Crops
			
			builder .HasMany(cs=>cs.Stages)
				.WithOne(c=>c.Crop)
				.HasForeignKey(c=>c.CropId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(c=>c.TotalActualCost)
				.HasColumnType("decimal(18,2)");

			builder.Property(c => c.TotalEstimatedCost)
				.HasColumnType("decimal(18,2)");

			builder.Property(c => c.PlanType)
				.HasConversion<string>(); // Store enum as string

		}
	}
}
