﻿using AgroMind.GP.Core.Contracts.Services.Contract;
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
	public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper,UserManager<AppUser>userManager,Func<ICartService> cartServiceFactory ) : IServiceManager
	{
		
		//Using Lazy Implementation

		//Lazy Attribute


		private readonly Lazy<IProductService> _LazyproductService= new Lazy<IProductService>( ()=> new ProductService(unitOfWork, mapper));

		private readonly Lazy<ICategoryService> _LazycategoryService = new Lazy<ICategoryService>(() => new CategoryService(unitOfWork, mapper));

		private readonly Lazy<IBrandService> _LazyBrandService = new Lazy<IBrandService>(() => new BrandService(unitOfWork, mapper));

		private readonly Lazy<ILandService> _LazyLandService = new Lazy<ILandService>(() => new LandService(unitOfWork, mapper,userManager));

		private readonly Lazy<ICropService> _LazyCropService = new Lazy<ICropService>(() => new CropService(unitOfWork, mapper,userManager));

		private readonly Lazy<IStageService> _LazyStageService = new Lazy<IStageService>(() => new StageService(unitOfWork, mapper,userManager));

		private readonly Lazy<IStepService> _LazyStepService = new Lazy<IStepService>(() => new StepService(unitOfWork, mapper,userManager));

		private readonly Lazy<ICartService> _LazyCartService = new Lazy<ICartService>(cartServiceFactory);




		//Create object From Service when u need it (Call Productservice)  accesss it (access el value)and create object of ProductService


		public IProductService ProductService => _LazyproductService.Value;

		public ICartService CartService => _LazyCartService.Value;

		public ICategoryService CategoryService => _LazycategoryService.Value;

		public IBrandService BrandService => _LazyBrandService.Value;

		public ILandService LandService => _LazyLandService.Value;

		public ICropService CropService => _LazyCropService.Value;

		public IStageService StageService => _LazyStageService.Value;

		public IStepService StepService => _LazyStepService.Value;



	}
}
