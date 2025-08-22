
using AgroMind.GP.APIs.CustomMiddleWares;
using AgroMind.GP.APIs.Extensions;
using AgroMind.GP.APIs.Factories;
using AgroMind.GP.APIs.Helpers;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Repository.Data.Contexts;
using AgroMind.GP.Repository.Data.SeedingData;
using AgroMind.GP.Repository.Repositories;
using AgroMind.GP.Service.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.ErrorModels;
using StackExchange.Redis;

namespace AgroMind.GP.APIs
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddHttpContextAccessor(); 
			builder.Services.AddDbContext<AgroMindContext>(Options =>
			{
				//Configuration >- el property el maska el file el appsetting
				Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

			});

			builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
			{
				options.TokenLifespan = TimeSpan.FromHours(2); // Set your desired expiration
			});
			#region IdentityServices

			//builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
			//{
			//	Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

			//});
			builder.Services.AddIdentityServices(builder.Configuration); //Extension Method have Services of Identity
			#endregion
			
			builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
			{
				var connection = builder.Configuration.GetConnectionString("RedisConnection");
				if (string.IsNullOrWhiteSpace(connection))
				{
					throw new InvalidOperationException("Redis connection string 'RedisConnection' is not configured.");
				}
				return ConnectionMultiplexer.Connect(connection);
			});

			builder.Services.AddScoped<ICartRepository, CartRepository>();
			//This AddScoped For Generic to didn't Add Service for each Repository
			builder.Services.AddScoped(typeof(IGenericRepositories<,>), typeof(GenericRepository<,>));
			//builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
			builder.Services.AddAutoMapper(typeof(MappingProfiles));

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


			
			builder.Services.AddScoped<IServiceManager, ServiceManager>();

			//builder.Services.AddScoped<IProductService, ProductService>();

			//builder.Services.AddScoped<ICategoryService, CategoryService>();

			//builder.Services.AddScoped<IBrandService, BrandService>();
			
			builder.Services.AddScoped(typeof(Func<ICartService>), (serviceProvider) =>
			{
				var mapper = serviceProvider.GetRequiredService<IMapper>();
				var CartRepo = serviceProvider.GetRequiredService<ICartRepository>();
				var Config = serviceProvider.GetRequiredService<IConfiguration>();
				return () => new CartService(CartRepo,mapper,Config);
			});

			builder.Services.AddScoped<ITokenService, TokenService>(); //  to register TokenService

			builder.Services.Configure<ApiBehaviorOptions>((options) =>
			{
				options.InvalidModelStateResponseFactory = APIResponseFactory.GenerateApiValidationErrorResponse;
			});

			builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
		options.JsonSerializerOptions.PropertyNamingPolicy = null;
		options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
		//options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonConverter<DateTime>());
	});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder => builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			});

			//Add all services BEFORE builder.Build()

			var app = builder.Build();

			

			#region Update DB
			//To Allow CLR To Inject Object From AgroMindDbContext
			using var Scope = app.Services.CreateScope(); //Cretae Scope : is Container has Servises Of LifeTime Type :Scoped
														  //Like :AgroMindDbContext() "Act Db"


			var Services = Scope.ServiceProvider;
			
			var context = Services.GetRequiredService<AgroMindContext>();
			var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
			//var logger = Services.GetRequiredService<ILogger<Program>>();
			var logger = loggerFactory.CreateLogger<Program>();
		
			var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = Services.GetRequiredService<UserManager<AppUser>>();

			//var validationKeyFromConfig = app.Configuration["JWT:key"]; // Use app.Configuration to access settings after build
			//if (string.IsNullOrEmpty(validationKeyFromConfig))
			//{
			//	logger.LogError("Program.cs: JWT:key is missing or empty in the configuration for validation!");
			//}
			//else
			//{
			//	logger.LogInformation($"Program.cs: JWT Key from config for validation: '{validationKeyFromConfig}' (Length: {validationKeyFromConfig.Length})");
			//}


			try // if DB kant Mawgoda
			{

				await context.Database.MigrateAsync(); //Update-Database

				await AppIdentityDbContextSeed.SeedRolesAsync(roleManager, logger);
				await AppIdentityDbContextSeed.SeedUserAsync(userManager, roleManager, logger);
				await AgroContextSeed.SeedAsync(context); //Seeding Data
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "There Are Problems during Apply Migrations !");// What Message Act => LogError -> red and Message of error
			}
			#endregion


			//builder.Logging.AddConsole();
			//builder.Logging.SetMinimumLevel(LogLevel.Debug);

			app.UseMiddleware<CustomExceptionHandlerMiddleWare>(); // Custom Middleware for Exception Handling

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseHttpsRedirection();// Redirects HTTP to HTTPS
			app.UseCors("AllowAll"); // Place CORS

		
			app.UseRouting();
			
			//app.UseStaticFiles();
			app.UseAuthentication();// Processes JWT token
			app.UseAuthorization();// Checks roles based on processed token
			app.MapControllers();// Maps routes


			


			app.Run();
		}
	}
}
