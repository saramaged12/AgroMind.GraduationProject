using AgroMind.GP.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace AgroMind.GP.Repository.HelperFunction
{
    public static class SoftDeleteQueryExtension
    {
        public static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
        {
			// Apply soft-delete filter to ALL entities
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
				// Only apply to root entities (not TPT-derived types)
				if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType)&&
					entityType.BaseType == null) // Only apply to root entities
                {
                    // Create filter expression: e => !e.IsDeleted
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var filter = Expression.Lambda(
                        Expression.Equal(property, Expression.Constant(false)),
                        parameter
                    );

                    entityType.SetQueryFilter(filter);
                }
            }
        }
    }
}
