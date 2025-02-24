using AgroMind.GP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Configurations
{
	public class CropConfiguration : IEntityTypeConfiguration<Crop>
	{
		public void Configure(EntityTypeBuilder<Crop> builder)
		{
			builder.HasOne(c => c.Farmer)
				   .WithMany(f=>f.Crops)
				   .HasForeignKey(c => c.FarmerId)
				   .OnDelete(DeleteBehavior.Cascade); // If Farmer is deleted delete Crops

		}
	}
}
