using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Specifications.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AgroMind.GP.Repository.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepositories<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
	{
		private readonly AgroMindContext _context;

		public GenericRepository(AgroMindContext context)
		{
			_context = context;
		}

		public async Task AddAsync(TEntity entity)=>
		
			await _context.Set<TEntity>().AddAsync(entity);

		public void Update(TEntity entity) =>
			_context.Set<TEntity>().Update(entity);



		public void SoftDelete(TEntity entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			entity.IsDeleted = true;
				entity.DeletedAt = DateTime.UtcNow;
				_context.Entry(entity).State = EntityState.Modified;
			
		}
		
		#region WithoutSpec
		public async Task<IReadOnlyList<TEntity>> GetAllAsync()
		{
			//IF there is NavigationProperty  should be use Include 
			//if (typeof(TEntity) == typeof(Product))

			//	return (IReadOnlyList<TEntity>)await _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(s => s.Supplier).ToListAsync();

			return await _context.Set<TEntity>().ToListAsync(); //Set<> // b t return DBSet of Any Type
		}
		public async Task<TEntity> GetByIdAsync(Tkey id)
		{
			return await _context.Set<TEntity>().FindAsync(id);
		}

		public async Task<IReadOnlyList<TEntity>> GetAllDeletedAsync()
		{
			return await _context.Set<TEntity>()
				.IgnoreQueryFilters()
				.Where(e => e.IsDeleted)
				.ToListAsync();
		}
		#endregion

		#region withSpec >- Open for Extension Closed for Modification
		public async Task<IReadOnlyList<TEntity>> GetAllWithSpecASync(ISpecification<TEntity, Tkey> spec)
		{
			return await ApplySpecification(spec).ToListAsync();
		}

		public async Task<TEntity> GetByIdAWithSpecAsync(ISpecification<TEntity, Tkey> spec)
		{
			return await ApplySpecification(spec).FirstOrDefaultAsync();
		}

		private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, Tkey> Spec)
		{
			return SpecificationEvaluator<TEntity, Tkey>.GetQuery(_context.Set<TEntity>(), Spec);
		}
		#endregion

	}
}
