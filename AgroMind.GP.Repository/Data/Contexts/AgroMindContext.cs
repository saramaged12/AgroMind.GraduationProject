using AgroMind.GP.Core.Contracts;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Repository.HelperFunction;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;
using System.Reflection;

namespace AgroMind.GP.Repository.Data.Contexts
{
	public class AgroMindContext : IdentityDbContext<AppUser>
	{
		public AgroMindContext(DbContextOptions<AgroMindContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Identity Default Configurations
			base.OnModelCreating(modelBuilder);

			// Apply soft-delete query filters (NEW)
			modelBuilder.ApplySoftDeleteQueryFilter();

			//3. Apply Entity Configurations

			//modelBuilder.ApplyConfiguration(new ProductConfigration()); // for one class configuration
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//will aplly of all configurations Classes That implement IEntityTypeConfigurations


			//4. Configure TPT inheritance
			// Apply Table-Per-Type (TPT)
			modelBuilder.Entity<Farmer>().ToTable(nameof(Farmer));
			modelBuilder.Entity<AgriculturalExpert>().ToTable(nameof(AgriculturalExpert));
			modelBuilder.Entity<SystemAdministrator>().ToTable(nameof(SystemAdministrator));
			modelBuilder.Entity<Supplier>().ToTable(nameof(Supplier));

			// 5. Configure value conversions
			ConfigureTimeSpanConversion(modelBuilder);


			// 6. Add indexes for query performance
			AddSoftDeleteIndexes(modelBuilder);
		}

		private void ConfigureTimeSpanConversion(ModelBuilder modelBuilder)
		{
			var timeSpanConverter = new ValueConverter<List<TimeSpan>, string>(
				v => string.Join(',', v.Select(ts => ts.ToString())),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
					  .Select(TimeSpan.Parse)
					  .ToList());

			var timeSpanComparer = new ValueComparer<List<TimeSpan>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList());

			modelBuilder.Entity<AgriculturalExpert>()
				.Property(e => e.AvailableHours)
				.HasConversion(timeSpanConverter)
				.Metadata.SetValueComparer(timeSpanComparer);
		}

		private void AddSoftDeleteIndexes(ModelBuilder modelBuilder)
		{
			// Add indexes for all soft-delete entities except user hierarchy
			modelBuilder.Entity<Address>().HasIndex(a => a.IsDeleted);
			modelBuilder.Entity<Product>().HasIndex(p => p.IsDeleted);
			modelBuilder.Entity<Category>().HasIndex(c => c.IsDeleted);
			modelBuilder.Entity<Crop>().HasIndex(c => c.IsDeleted);
			modelBuilder.Entity<Land>().HasIndex(l => l.IsDeleted);
			modelBuilder.Entity<Brand>().HasIndex(b => b.IsDeleted);
			modelBuilder.Entity<CropStage>().HasIndex(cs => cs.IsDeleted);
			modelBuilder.Entity<Step>().HasIndex(s => s.IsDeleted);
			modelBuilder.Entity<RecommendRequest>().HasIndex(r => r.IsDeleted);
			
			// User indexes (only on AppUser root entity)
			modelBuilder.Entity<AppUser>().HasIndex(u => u.IsDeleted);
		}


		public DbSet<Address> Addresss { get; set; }
		public DbSet<Farmer> Farmers { get; set; }
		public DbSet<AgriculturalExpert> AgriculturalExperts { get; set; }
		public DbSet<SystemAdministrator> SystemAdministrators { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }

		public DbSet<Brand> Brands { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Crop> Crop { get; set; }
		public DbSet<CropStage> CropStage { get; set; }

		public DbSet<Step> Step { get; set; }

		public DbSet<Land> Land { get; set; }

		public DbSet<RecommendRequest> RecommendPlan { get; set; }




	}
}
