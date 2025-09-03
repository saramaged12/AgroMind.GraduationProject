using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace AgroMind.GP.Service.Services
{
	public class ServiceManager(Func<IProductService> productServiceFactory,
			Func<ICategoryService> categoryServiceFactory,
			Func<IBrandService> brandServiceFactory,
			Func<ILandService> landServiceFactory,
			Func<ICropService> cropServiceFactory,
			Func<IStageService> stageServiceFactory,
			Func<IStepService> stepServiceFactory,
		Func<IOrderService> OrderServiceFactory, Func<ICartService> cartServiceFactory ) : IServiceManager
	{
		
		//Using Lazy Implementation

		//Lazy Attribute


		private readonly Lazy<IProductService> _LazyproductService= new Lazy<IProductService>(productServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<ICategoryService> _LazycategoryService = new Lazy<ICategoryService>(categoryServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<IBrandService> _LazyBrandService = new Lazy<IBrandService>(brandServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<ILandService> _LazyLandService = new Lazy<ILandService>(landServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<ICropService> _LazyCropService = new Lazy<ICropService>(cropServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<IStageService> _LazyStageService = new Lazy<IStageService>(stageServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<IStepService> _LazyStepService = new Lazy<IStepService>(stepServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<ICartService> _LazyCartService = new Lazy<ICartService>(cartServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Lazy<IOrderService> _LazyOrderService = new Lazy<IOrderService> (OrderServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);


		//Create object From Service when u need it (Call Productservice)  accesss it (access el value)and create object of ProductService


		public IProductService ProductService => _LazyproductService.Value;

		public ICartService CartService => _LazyCartService.Value;

		public ICategoryService CategoryService => _LazycategoryService.Value;

		public IBrandService BrandService => _LazyBrandService.Value;

		public ILandService LandService => _LazyLandService.Value;

		public ICropService CropService => _LazyCropService.Value;

		public IStageService StageService => _LazyStageService.Value;

		public IStepService StepService => _LazyStepService.Value;

		public IOrderService OrderService => _LazyOrderService.Value;



	}
}
