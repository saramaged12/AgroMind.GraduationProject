using AgroMind.GP.Core.Contracts.Specifications.Contract;
using AgroMind.GP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AgroMind.GP.Repository
{

    public static class SpecificationEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
	{
		//Function Will Build Dynamic Query

		//context.Products.Where(P=>P.Id==id as int?).Include(p => p.Brand).Include(p => p.BrandType).FirstOrDefaultAsync(p=>p.Id==id as int ?) as TEntity;

		//IQueryable //Filteration in DB & IEnummerable //Filteration in Application
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> Spec)
		{
			var Query = inputQuery; // context.Set<T>()

			// Apply filtering (Criteria)
			if (Spec.Criteria is not null) // P=>P.Id==id 
			{
				Query = Query.Where(Spec.Criteria); // context.Set<T>().where (P=>P.Id==id) 
			}

			if (Spec.OrderBy is not null)
			{
				Query = Query.OrderBy(Spec.OrderBy); // context.Set<T>().where (P=>P.Id==id).OrderBy(P=>P.Name)
			}

			if (Spec.OrderByDescending is not null)
			{
				Query = Query.OrderByDescending(Spec.OrderByDescending); // context.Set<T>().where (P=>P.Id==id).OrderByDescending(P=>P.Name)
			}
			//p => p.Brand

			//p => p.BrandType

			Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

			//context.Set<T>().where (P=>P.Id==id) //CurrentQuery
			//Include(p => p.Brand) => IncludeExpression
			//CurrentQuery.Include(p => p.Brand) => CurrentQuery
			//CuurentQuery.Include(p => p.BrandType)


			// Apply string-based Includes (for ThenIncludes)
			foreach (var include in Spec.StringIncludes)
			{
				Query = Query.Include(include);
			}
			return Query;
		}
	}
}
