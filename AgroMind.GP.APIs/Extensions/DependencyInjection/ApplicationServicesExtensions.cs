using AgroMind.GP.APIs.Factories;
using AgroMind.GP.APIs.Mapping;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Repository.Repositories;
using AgroMind.GP.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Extensions.DependencyInjection
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration config)
		{
			// 1. AutoMapper Registration
			// builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
		     Services.AddAutoMapper(typeof(MappingProfiles));

			// 2. Repositories Registration
			Services.AddScoped<ICartRepository, CartRepository>();

			//This AddScoped For Generic to didn't Add Service for each Repository
			Services.AddScoped(typeof(IGenericRepositories<,>), typeof(GenericRepository<,>));

			Services.AddScoped<IUnitOfWork, UnitOfWork>();


			// 3. Concrete Services Registration 
		
			Services.AddScoped<IProductService, ProductService>();
			Services.AddScoped<ICategoryService, CategoryService>();
			Services.AddScoped<IBrandService, BrandService>();
			Services.AddScoped<ILandService, LandService>();
			Services.AddScoped<ICropService, CropService>();
			Services.AddScoped<IStageService, StageService>();
			Services.AddScoped<IStepService, StepService>();
			Services.AddScoped<ICartService, CartService>();
			Services.AddScoped<IOrderService, OrderService>(); 
			Services.AddScoped<ITokenService, TokenService>(); 


			// 4. Func Factories Registration

			Services.AddScoped(typeof(Func<ICartService>), (serviceProvider) =>
			{

				return () => serviceProvider.GetRequiredService<ICartService>();
			});


			Services.AddScoped(typeof(Func<IOrderService>), (serviceProvider) =>
			{

				return () => serviceProvider.GetRequiredService<IOrderService>();
			});

			Services.AddScoped(typeof(Func<IProductService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<IProductService>();
			});

			Services.AddScoped(typeof(Func<ICategoryService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<ICategoryService>();
			});

			Services.AddScoped(typeof(Func<IBrandService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<IBrandService>();
			});

			Services.AddScoped(typeof(Func<ILandService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<ILandService>();
			});

			Services.AddScoped(typeof(Func<ICropService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<ICropService>();
			});

			Services.AddScoped(typeof(Func<IStageService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<IStageService>();
			});

			Services.AddScoped(typeof(Func<IStepService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<IStepService>();
			});

			// 5. ServiceManager Registration

			Services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

			// 6. API Behavior Options for invalid model state
			Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = APIResponseFactory.GenerateApiValidationErrorResponse;
			});

			// 7. HttpContextAccessor Registration
			Services.AddHttpContextAccessor();

			return Services;
		}

	}
}
