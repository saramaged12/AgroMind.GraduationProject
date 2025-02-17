using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Repository.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Contexts
{
	public class AgroMindContext : IdentityDbContext<AppUser>
	{
		public AgroMindContext(DbContextOptions<AgroMindContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.ApplyConfiguration(new ProductConfigration()); // for one class configuration
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//will aplly of all configurations
			//fluent api
			base.OnModelCreating(modelBuilder);
		}

		
		public DbSet<Address> Addresss { get; set; }
	}
}
