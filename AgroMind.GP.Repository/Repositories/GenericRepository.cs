using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
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

		public void Delete(TEntity entity)
		{
			_context.Remove(entity);
		}

		public async  Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _context.Set<TEntity>().ToListAsync(); //Set<> // b t return DBSet of Any Type
		}
		public async Task<TEntity> GetByIdAsyn(int id)
		{
			return await _context.Set<TEntity>().FindAsync(id);
		}

		public Task<TEntity> GetByIdAsyn(Tkey id)
		{
			throw new NotImplementedException();
		}

		public void Update(TEntity entity)
		{
			_context.Update(entity);
		}
	}
}
