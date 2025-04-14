using AgroMind.GP.APIs.Extensions;
using AgroMind.GP.APIs.Helpers;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Repository.Data.Contexts;
using AgroMind.GP.Repository.Data.SeedingData;
using AgroMind.GP.Repository.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AgroMind.GP.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure main database
            builder.Services.AddDbContext<AgroMindContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Optional: Identity DB context (only if using separate DB for users)
            // builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            // {
            //     options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            // });

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            // Identity services
            builder.Services.AddIdentityServices(builder.Configuration);

            // Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });

            // Repositories
            builder.Services.AddScoped(typeof(IGenericRepositories<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped<ICartRepository, CartRepository>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            var app = builder.Build();

            app.UseCors("AllowAll");

            // Apply database migrations and seed data
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AgroMindContext>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                await context.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager, logger);
                await AppIdentityDbContextSeed.SeedUserAsync(userManager, roleManager, logger);
                await AgroContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration/seeding.");
            }

            // Middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
