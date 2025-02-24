using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Core.Specifications.Contract;
using AgroMind.GP.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Repositories
{
	public class GenericRepository<TEntity,Tkey> : IGenericRepositories <TEntity,Tkey> where TEntity : BaseEntity<Tkey>
	{
		private readonly AgroMindContext _context;

		public GenericRepository(AgroMindContext context)
		{
			_context = context;
		}

		public async Task AddAsync(TEntity entity)
		{
			 await _context.AddAsync(entity);
		}
		public void Update(TEntity entity)
		{
			_context.Update(entity);
		}

		public void Delete(TEntity entity)
		{
			_context.Remove(entity);
		}

		#region WithoutSpec
		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			//IF there is NavigationProperty  should be use Include 
			if (typeof(TEntity) == typeof(Product))

				return (IEnumerable<TEntity>)await _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(s => s.Supplier).ToListAsync();

			return await _context.Set<TEntity>().ToListAsync(); //Set<> // b t return DBSet of Any Type
		}
		public async Task<TEntity> GetByIdAsync(Tkey id)
		{
			return await _context.Set<TEntity>().FindAsync(id);
		} 
		#endregion

		#region withSpec >- Open for Extension Closed for Modification
		public async Task<IEnumerable<TEntity>> GetAllWithSpecASync(ISpecification<TEntity, Tkey> spec)
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
		public async Task DeleteWithSpecAsync(ISpecification<TEntity, Tkey> spec)
		{
			var entity = await GetByIdAWithSpecAsync(spec);
			if (entity != null)
			{
				_context.Set<TEntity>().Remove(entity);
				//await _context.SaveChangesAsync(); // Ensure changes are saved to the database
			}
		}
	}
}
