using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Repository.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Contexts
{
	public class LandContext : DbContext
	{
		public LandContext(DbContextOptions<LandContext> options) : base(options)
		{}

        public DbSet<Land> Lands { get; set; }
        //public DbSet<Crop> Crops { get; set; }
        //public DbSet<Farmer> Farmers { get; set; }
        //public DbSet<Agent> Agents { get; set; }   1:M relationship
    }
}
