using AgroMind.GP.Core.Entities;
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
		Task<IEnumerable<TEntity>>GetAllAsync();
		Task<TEntity>GetByIdAsyn(Tkey id);
		Task AddAsync(TEntity entity);
		void Update(TEntity entity); //Update and Delete Not Work Async
		void Delete(TEntity entity);
	}
}
