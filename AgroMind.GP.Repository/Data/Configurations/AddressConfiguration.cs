using AgroMind.GP.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Configurations
{
	public class AddressConfiguration : IEntityTypeConfiguration<Address>
	{
		public void Configure(EntityTypeBuilder<Address> builder)
		{
			
			builder.HasKey(a => a.Id);

			

			builder.HasOne(a => a.AppUser)
				.WithOne(u => u.Address)
				.HasForeignKey<Address>(a => a.Id)
				.OnDelete(DeleteBehavior.Cascade); // If User is deleted, delete Addresses
		}

	}


}