using AgroMind.GP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroMind.GP.Repository.Data.Configurations
{
	public class CropStageConfiguration : IEntityTypeConfiguration<CropStage>
	{
		public void Configure(EntityTypeBuilder<CropStage> builder)
		{
			builder.HasOne(cs => cs.Crop)
					.WithMany(C => C.Stages)
					.HasForeignKey(S => S.CropId)
			.OnDelete(DeleteBehavior.Restrict);  // If Crop is deleted, keep StageCrops
		}
	}
}
