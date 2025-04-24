using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Repository.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace AgroMind.GP.Service.Services
{
	public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper) : IServiceManager
	{
		
		//Using Lazy Implementation

		//Lazy Attribute


		private readonly Lazy<IProductService> _LazyproductService= new Lazy<IProductService>( ()=> new ProductService(unitOfWork, mapper));

		private readonly Lazy<ICategoryService> _LazycategoryService = new Lazy<ICategoryService>(() => new CategoryService(unitOfWork, mapper));

		private readonly Lazy<IBrandService> _LazyBrandService = new Lazy<IBrandService>(() => new BrandService(unitOfWork, mapper));


		//Create object From ProductService when u need it (Call Productservice)  accesss it (access el value)and create object of ProductService


		public IProductService ProductService => _LazyproductService.Value;

		public ICategoryService CategoryService => _LazycategoryService.Value;

		public IBrandService BrandService => _LazyBrandService.Value;
	}
}
