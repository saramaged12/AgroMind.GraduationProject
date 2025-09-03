
using AgroMind.GP.APIs.CustomMiddleWares;
using AgroMind.GP.APIs.Extensions.DependencyInjection;
using AgroMind.GP.APIs.Factories;
using AgroMind.GP.APIs.Mapping;
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

		
			builder.Services.AddControllers()
	              .AddJsonOptions(options =>
		          { 
	    	      options.JsonSerializerOptions.Converters.Add(new
						System.Text.Json.Serialization.JsonStringEnumConverter());
	    	      options.JsonSerializerOptions.PropertyNamingPolicy = null;
		          options.JsonSerializerOptions.DefaultIgnoreCondition =
					    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
		          //options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonConverter<DateTime>());
	             });



			//Add all services BEFORE builder.Build()

			//Service Registrations 

			builder.Services.AddSwaggerDocumentation();             // From SwaggerExtensions
			builder.Services.AddDatabaseAndCacheServices(builder.Configuration); // From DatabaseAndCacheExtensions
			builder.Services.AddIdentityServices(builder.Configuration); // From IdentityServiceExtensions
			builder.Services.AddApplicationServices(builder.Configuration); // From ApplicationServicesExtensions
			builder.Services.AddCorsPolicies();

			var app = builder.Build();



			#region  Database Migration and Seeding
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


			// Middleware Pipeline Configuration 

			app.UseMiddleware<CustomExceptionHandlerMiddleWare>(); // Custom Middleware for Exception Handling

			app.UseStatusCodePagesWithReExecute("/errors/{0}"); // {0} is the status code

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerDocumentation(); // From SwaggerExtensions
			}
			app.UseHttpsRedirection();// Redirects HTTP to HTTPS
			app.UseRouting();
			app.UseCors("AllowAll"); // Place CORS

			//app.UseStaticFiles();
			app.UseAuthentication();// Processes JWT token
			app.UseAuthorization();// Checks roles based on processed token
			app.MapControllers();// Maps routes


			app.Run();
		}
	}
}
