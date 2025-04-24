using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AgroMindContext _context;

		public UnitOfWork(AgroMindContext context)
		{
			_context = context;
		}
		//Dependency Injection: The AgroMindContext is injected into the constructor
		//	,allowing the UnitOfWork to interact with the database.

		private readonly Dictionary<string, object> _repositories =  new Dictionary<string,object>() ;

		//This dictionary is used to store instances of repositories.
		//The key is the name of the entity type (TypeName)
		//and the value is the repository instance.
		//This ensures that only one instance of a repository
		//is created for each entity type during the lifetime of the Unit Of Work

		public IGenericRepositories<TEntity, TKey> GetRepositories<TEntity, TKey>() where TEntity:BaseEntity<TKey>
		{
			//This method provides a repository instance for
			//a specific entity type (TEntity) and its key type (TKey).

			//Get Entity Type Name
			var TypeName = typeof(TEntity).Name;

			//Dic<string,object> ==>string Key [Name Of Type] -- Object Object From Generic Repository

			//Retrieves the name of the entity type( Product, Order, ....).

			if (_repositories.TryGetValue(TypeName, out object? value) ) 
				return (IGenericRepositories<TEntity, TKey>) _repositories[TypeName];

			else
			{
				//create object
				var Repo= new GenericRepository<TEntity,TKey>(_context);

				//store object in Dic

				_repositories[TypeName] = Repo;
				//return object

				return Repo;
			}
		}


		public async Task<int> SaveChangesAsync()=>
		
			await _context.SaveChangesAsync();
		
	}
	
}
