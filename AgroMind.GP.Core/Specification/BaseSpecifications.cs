using AgroMind.GP.Core.Contracts.Specifications.Contract;
using AgroMind.GP.Core.Entities;
using System.Linq.Expressions;

namespace AgroMind.GP.Core.Specification
{
	public class BaseSpecifications<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
	{


		//Automatic Properties


		public Expression<Func<TEntity, bool>>? Criteria { get; set; }

		#region Include
		public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();

		public List<string> StringIncludes { get; set; } = new List<string>(); // For string-based includes


		protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)

				=> Includes.Add(includeExpression);

		protected void AddThenInclude<TPreviousProperty>(
			Expression<Func<TEntity, IEnumerable<TPreviousProperty>>> includeExpression,
			Expression<Func<TPreviousProperty, object>> thenIncludeExpression)
		{
			// Add a string representation for ThenIncludes (EF Core handles them automatically)
			StringIncludes.Add($"{includeExpression.Body}.{thenIncludeExpression.Body}");
		}

		#endregion

		#region Sorting
		public Expression<Func<TEntity, object>> OrderBy { get; private set; }
		public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

		protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) => OrderBy = orderByExpression;

		protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression) => OrderByDescending = orderByDescExpression;
		#endregion

		//Get All
		public BaseSpecifications()
		{
			//Includes = new List<Expression<Func<TEntity, object>>>();
		}

		//Get By ID
		public BaseSpecifications(Expression<Func<TEntity, bool>>? criteriaExpression)
		{
			Criteria = criteriaExpression;
			//Includes=new List<Expression<Func<TEntity,object>>>();	
		}

		
	}


}
