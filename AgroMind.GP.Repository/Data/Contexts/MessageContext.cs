using AgroMind.GP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Data.Contexts
{
    class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        //public DbSet<Farmer> Farmers { get; set; }  //M:1
        //public DbSet<AgriculturalExpert> AgriculturalExperts { get; set; }  //M:1
        //public DbSet<Agent> Agents { get; set; }  //M:1
    }
}
