using AgroMind.GP.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AgroMind.GP.APIs.Extensions.DependencyInjection
{
	public static class DatabaseAndCacheExtensions
	{
		public static IServiceCollection AddDatabaseAndCacheServices(this IServiceCollection services, IConfiguration config)
		{
			// Add DbContext for SQL Server
			services.AddDbContext<AgroMindContext>(options =>
			{
				options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
			});

			// Add Redis
			services.AddSingleton<IConnectionMultiplexer>(sp =>
			{
				var connection = config.GetConnectionString("RedisConnection");
				if (string.IsNullOrWhiteSpace(connection))
				{
					throw new InvalidOperationException("Redis connection string 'RedisConnection' is not configured.");
				}
				return ConnectionMultiplexer.Connect(connection);
			});

			return services;
		}
	}


}
