using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.UnitOfWork.Contract
{
	public interface IUnitOfWork
	{
		//Signature for Property For each Repository

		//public IGenericRepositories<Land,int> LandRepository {  get; }

		//Function will Generate object of Repository :
		IGenericRepositories<TEntity,TKey> GetRepositories<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

		Task<int> SaveChangesAsync();
	}
}
