using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Repository.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Contexts
{
	public class AgroMindContext : IdentityDbContext<AppUser>
	{
		public AgroMindContext(DbContextOptions<AgroMindContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.ApplyConfiguration(new ProductConfigration()); // for one class configuration
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//will aplly of all configurations

			//Identity Default Configurations
			base.OnModelCreating(modelBuilder);

			//fluent api

			// Apply Table-Per-Type (TPT)
			modelBuilder.Entity<Farmer>().ToTable(nameof(Farmer));
			modelBuilder.Entity<AgriculturalExpert>().ToTable(nameof(AgriculturalExpert));
			modelBuilder.Entity<SystemAdministrator>().ToTable(nameof(SystemAdministrator));
			modelBuilder.Entity<Supplier>().ToTable(nameof(Supplier));

			// Handle AvailableHours (TimeSpan List → Stored as a string in SQL)
			var timeSpanConverter = new ValueConverter<List<TimeSpan>, string>(
				v => string.Join(',', v.Select(ts => ts.ToString())),  // Convert List<TimeSpan> -> string
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
					  .Select(TimeSpan.Parse)
					  .ToList() // Convert string -> List<TimeSpan>
			);

			var timeSpanComparer = new ValueComparer<List<TimeSpan>>(
				(c1, c2) => c1.SequenceEqual(c2),  // Compare lists
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // HashCode
				c => c.ToList()  // Snapshot copy
			);

			modelBuilder.Entity<AgriculturalExpert>()
				.Property(e => e.AvailableHours)
				.HasConversion(timeSpanConverter)
				.Metadata.SetValueComparer(timeSpanComparer);
		}

		////Table-Per-Type (TPT) Mapping
		//modelBuilder.Entity<Farmer>().ToTable(nameof(Farmer)).HasBaseType<AppUser>();
		//	modelBuilder.Entity<AgriculturalExpert>().ToTable(nameof(AgriculturalExpert)).HasBaseType<AppUser>();
		//	modelBuilder.Entity<SystemAdministrator>().ToTable(nameof(SystemAdministrator)).HasBaseType<AppUser>();
		//	modelBuilder.Entity<Supplier>().ToTable(nameof(Supplier)).HasBaseType<AppUser>();

		//	//Handle List<TimeSpan> for AvailableHours in Expert
		//	var timeSpanConverter = new ValueConverter<List<TimeSpan>, string>(
		//		v => string.Join(',', v.Select(ts => ts.ToString())),  // Convert List<TimeSpan> -> string
		//		v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
		//			  .Select(TimeSpan.Parse)
		//			  .ToList() // Convert string -> List<TimeSpan>
		//	);
		//	var timeSpanComparer = new ValueComparer<List<TimeSpan>>(
		//		(c1, c2) => c1.SequenceEqual(c2),  // Compare two lists for equality
		//		c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Generate hash
		//		c => c.ToList()  // Create a snapshot copy for EF Core tracking
		//	);

		//	modelBuilder.Entity<AgriculturalExpert>()
		//		.Property(e => e.AvailableHours)
		//		.HasConversion(timeSpanConverter)
		//		.Metadata.SetValueComparer(timeSpanComparer);

		//AvailableHours : EF Core will store AvailableHours as a string in SQL ("09:00:00,15:00:00")
		//and convert it back to List<TimeSpan> when queried



		// Configure relationships
		//modelBuilder.Entity<Farmer>()
  //          .HasMany(f => f.Crops)
  //          .WithOne() // Assuming Crop has a Farmer navigation property
  //          .HasForeignKey(c => c.FarmerId);

		//modelBuilder.Entity<Farmer>()
  //          .HasMany(f => f.Lands)
  //          .WithOne() // Assuming Land has a Farmer navigation property
  //          .HasForeignKey(l => l.FarmerId);

		//modelBuilder.Entity<Supplier>()
  //          .HasMany(s => s.Products)
  //          .WithOne() // Assuming Product has a Supplier navigation property
  //          .HasForeignKey(p => p.SupplierId);

		//modelBuilder.Entity<AgriculturalExpert>()
  //          .HasMany(e => e.Consultations)
  //          .WithOne() // Assuming Consultation has an Expert navigation property
  //          .HasForeignKey(c => c.ExpertId);



		public DbSet<Address> Addresss { get; set; }
		public DbSet<Farmer> Farmers { get; set; }
		public DbSet<AgriculturalExpert> AgriculturalExperts { get; set; }
		public DbSet<SystemAdministrator> SystemAdministrators { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }

	}
}
