using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Specifications.Contract;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Repositories.Contract
{
	public interface IGenericRepositories<TEntity,Tkey> where TEntity : BaseEntity<Tkey> 
		//To Mathc Any Type of Key(String or Int)
	{
		#region withoutSpec
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity> GetByIdAsync(Tkey id); 
		#endregion

		#region With Specification
		Task<IEnumerable<TEntity>> GetAllWithSpecASync(ISpecification<TEntity,Tkey> spec);

		Task<TEntity> GetByIdAWithSpecAsync(ISpecification<TEntity,Tkey> spec);

	 //   Task DeleteWithSpecAsync(ISpecification<TEntity, Tkey> spec);

		//Task UpdateWithSpecAsync(ISpecification<TEntity, Tkey> spec ,Action<TEntity> action);

		#endregion
		Task AddAsync(TEntity entity);
		void Update(TEntity entity); //Update and Delete Not Work Async
		void Delete(TEntity entity);
	}
}
