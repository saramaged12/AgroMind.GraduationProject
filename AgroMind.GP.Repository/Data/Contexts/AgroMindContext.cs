using AgroMind.GP.Core.Contracts;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Repository.HelperFunction;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Shared.DTOs;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

namespace AgroMind.GP.Repository.Data.Contexts
{
	public class AgroMindContext : IdentityDbContext<AppUser>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AgroMindContext(DbContextOptions<AgroMindContext> options, IHttpContextAccessor httpContextAccessor)
			: base(options)
		{
			_httpContextAccessor = httpContextAccessor;
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




		// --- SaveChanges Overrides for Audit Fields ---
		public override int SaveChanges()
		{
			ApplyAuditInformation();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			ApplyAuditInformation();
			return await base.SaveChangesAsync(cancellationToken);
		}

		private void ApplyAuditInformation()
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is BaseEntity<int> auditableEntity)
				{
					switch (entry.State)
					{
						case EntityState.Added:
							if (string.IsNullOrEmpty(auditableEntity.CreatorId))
							{
								auditableEntity.CreatorId = currentUserId;
							}
							//auditableEntity.CreatedAt = DateTime.UtcNow;
							
							break;

						case EntityState.Modified:
							// For existing entities, ensure CreatedAt and CreatorId are NOT overwritten
							entry.Property(nameof(BaseEntity<int>.CreatorId)).IsModified = false;
							//entry.Property(nameof(BaseEntity<int>.CreatedAt)).IsModified = false;
						
							break;

						case EntityState.Deleted:
							auditableEntity.IsDeleted = true;
							auditableEntity.DeletedAt = DateTime.UtcNow;
							entry.State = EntityState.Modified;
							break;
					}
				}
			}
		}

		// --- OnModelCreating ---
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplySoftDeleteQueryFilter();
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			// TPT inheritance
			modelBuilder.Entity<Farmer>().ToTable(nameof(Farmer));
			modelBuilder.Entity<AgriculturalExpert>().ToTable(nameof(AgriculturalExpert));
			modelBuilder.Entity<SystemAdministrator>().ToTable(nameof(SystemAdministrator));
			modelBuilder.Entity<Supplier>().ToTable(nameof(Supplier));

			ConfigureTimeSpanConversion(modelBuilder);
			AddSoftDeleteIndexes(modelBuilder);


			// --- Configure Audit Fields (Creator Only) Relationships ---
			// Pass the builder for each entity that inherits BaseEntity
			ConfigureCreatorRelationship(modelBuilder.Entity<Product>());
			ConfigureCreatorRelationship(modelBuilder.Entity<Category>());
			ConfigureCreatorRelationship(modelBuilder.Entity<Brand>());
			ConfigureCreatorRelationship(modelBuilder.Entity<Land>());
			ConfigureCreatorRelationship(modelBuilder.Entity<Crop>());
			ConfigureCreatorRelationship(modelBuilder.Entity<CropStage>());
			ConfigureCreatorRelationship(modelBuilder.Entity<Step>());
			
			//ConfigureCreatorRelationship(modelBuilder.Entity<RecommendRequest>()); // Add if RecommendRequest inherits BaseEntity

			modelBuilder.Entity<Address>()
			  .HasOne(a => a.Creator)
			  .WithMany()
			  .HasForeignKey(a => a.CreatorId)
			  .IsRequired(false)
			  .OnDelete(DeleteBehavior.Restrict);

		}

		// --- Helper method for Creator relationship ONLY ---
		private void ConfigureCreatorRelationship<TEntity>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity> builder)
			where TEntity : BaseEntity<int>
		{
			builder.HasOne(e => e.Creator)
				   .WithMany()
				   .HasForeignKey(e => e.CreatorId)
				   .IsRequired(false) // CreatorId can be null if needed (e.g., system-created data)
				   .OnDelete(DeleteBehavior.Restrict);
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
			// Add indexes for IsDeleted 
			modelBuilder.Entity<Address>().HasIndex(a => a.IsDeleted);
			modelBuilder.Entity<Product>().HasIndex(p => p.IsDeleted);
			modelBuilder.Entity<Category>().HasIndex(c => c.IsDeleted);
			modelBuilder.Entity<Crop>().HasIndex(c => c.IsDeleted);
			modelBuilder.Entity<Land>().HasIndex(l => l.IsDeleted);
			modelBuilder.Entity<Brand>().HasIndex(b => b.IsDeleted);
			modelBuilder.Entity<CropStage>().HasIndex(cs => cs.IsDeleted);
			modelBuilder.Entity<Step>().HasIndex(s => s.IsDeleted);
			
			modelBuilder.Entity<AppUser>().HasIndex(u => u.IsDeleted);

			// Add indexes for CreatorId 
			modelBuilder.Entity<Product>().HasIndex(p => p.CreatorId);
			modelBuilder.Entity<Category>().HasIndex(c => c.CreatorId);
			modelBuilder.Entity<Brand>().HasIndex(b => b.CreatorId);
			modelBuilder.Entity<Land>().HasIndex(l => l.CreatorId);
			modelBuilder.Entity<Crop>().HasIndex(c => c.CreatorId);
			modelBuilder.Entity<CropStage>().HasIndex(cs => cs.CreatorId);
			modelBuilder.Entity<Step>().HasIndex(s => s.CreatorId);
			modelBuilder.Entity<Address>().HasIndex(a => a.CreatorId);
		
		}
	}
}