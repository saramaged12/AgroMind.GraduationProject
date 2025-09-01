using AgroMind.GP.Core.Contracts;
using AgroMind.GP.Core.Contracts.Common;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.Orders;
using AgroMind.GP.Core.Entities.ProductModule;
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
		private string? _currentUserId; // Cache current user ID for performance

		public AgroMindContext(DbContextOptions<AgroMindContext> options, IHttpContextAccessor httpContextAccessor)
			: base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public DbSet<Addresses> Addresses { get; set; }
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

		public DbSet<Order> Orders { get; set; }	

		public DbSet<OrderItems> OrderItems { get; set; }

		public DbSet<DeliveryMethod> DeliveryMethods { get; set; }


		// --- SaveChanges Overrides for Audit Fields and Soft Delete ---
		public override int SaveChanges()
		{
			ApplyAuditAndSoftDeleteInformation();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			ApplyAuditAndSoftDeleteInformation();
			return await base.SaveChangesAsync(cancellationToken);
		}

		private void ApplyAuditAndSoftDeleteInformation()
		{
			// Get current user ID once per SaveChanges call
			_currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var currentTime = DateTime.UtcNow;

			foreach (var entry in ChangeTracker.Entries())
			{
				// Handle IAuditableEntity
				if (entry.Entity is IAuditableEntity auditableEntity)
				{
					switch (entry.State)
					{
						case EntityState.Added:
							auditableEntity.CreatedAt = currentTime;
							auditableEntity.CreatedBy = _currentUserId;
						
							auditableEntity.LastModifiedAt = currentTime;
							auditableEntity.LastModifiedBy = _currentUserId;
							break;

						case EntityState.Modified:
							auditableEntity.LastModifiedAt = currentTime;
							auditableEntity.LastModifiedBy = _currentUserId;
							entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
							entry.Property(nameof(IAuditableEntity.CreatedBy)).IsModified = false;
							break;
					}
				}

				// Handle ISoftDelete
				if (entry.Entity is ISoftDelete softDeleteEntity && entry.State == EntityState.Deleted)
				{
					softDeleteEntity.IsDeleted = true;
					softDeleteEntity.DeletedAt = currentTime;
					// Change state to Modified so it's updated in DB instead of truly deleted
					entry.State = EntityState.Modified;

					// If it's also auditable, update LastModified fields for soft delete
					if (entry.Entity is IAuditableEntity auditableSoftDeletedEntity)
					{
						auditableSoftDeletedEntity.LastModifiedAt = currentTime;
						auditableSoftDeletedEntity.LastModifiedBy = _currentUserId;
						// Ensure CreatedAt and CreatedBy are NOT overwritten
						entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
						entry.Property(nameof(IAuditableEntity.CreatedBy)).IsModified = false;
					}
				}
			}
		}

		// --- OnModelCreating ---
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Apply all configurations from the current assembly
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			// Centralized configuration for soft delete query filter
			ApplyGlobalSoftDeleteFilter(modelBuilder);

			// TPT (Table Per Type) inheritance for identity users
			modelBuilder.Entity<Farmer>().ToTable(nameof(Farmer));
			modelBuilder.Entity<AgriculturalExpert>().ToTable(nameof(AgriculturalExpert));
			modelBuilder.Entity<SystemAdministrator>().ToTable(nameof(SystemAdministrator));
			modelBuilder.Entity<Supplier>().ToTable(nameof(Supplier));

			// Specific conversions
			ConfigureTimeSpanConversion(modelBuilder);

			// Add indexes for common query fields
			AddCommonIndexes(modelBuilder);
		}


		// Applies a global query filter for soft-deleted entities.

		private void ApplyGlobalSoftDeleteFilter(ModelBuilder modelBuilder)
		{
			// Apply the soft delete filter specifically to the AppUser entity,
			// as it is the root of  identity inheritance hierarchy.
			
			modelBuilder.Entity<AppUser>().HasQueryFilter(e => !e.IsDeleted);

			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				
				if (typeof(AppUser).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(AppUser))
				{
					continue; 
				}

				if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
				{
					
					if (!typeof(AppUser).IsAssignableFrom(entityType.ClrType))
					{
						var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
						var property = System.Linq.Expressions.Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
						var filter = System.Linq.Expressions.Expression.Lambda(
							System.Linq.Expressions.Expression.Not(property), parameter);
						entityType.SetQueryFilter(filter);
					}
				}
			}
		}


		// Configures conversion for List<TimeSpan> property.

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

	
		// Adds common indexes for performance.
		
		private void AddCommonIndexes(ModelBuilder modelBuilder)
		{
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				// Index for IsDeleted
				if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
				{
					modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(ISoftDelete.IsDeleted));
				}

				// Index for CreatedBy
				if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
				{
					modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(IAuditableEntity.CreatedBy));
				}
				// Index for LastModifiedBy
				if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
				{
					modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(IAuditableEntity.LastModifiedBy));
				}
			}
			// Explicit index for AppUser's IsDeleted, as AppUser might not implement ISoftDelete directly
			modelBuilder.Entity<AppUser>().HasIndex(u => u.IsDeleted);
		}

	}
}