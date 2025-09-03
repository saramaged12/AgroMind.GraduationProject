namespace AgroMind.GP.APIs.Extensions.DependencyInjection
{
	public static  class CorsExtensions
	{
		public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder => builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			});
			return services;
		}
	}
}
