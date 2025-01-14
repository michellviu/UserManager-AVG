using Core.Domain.Entities;
using Infrastructure.Infrastructure.Persistence.Contexts.EntitiesConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure.Persistence.Contexts
{
    public class AppDBContext:IdentityDbContext<User,IdentityRole<int>,int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
     : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            modelBuilder.ApplyConfiguration(new UserConfiguration());
         
        }
    }
}
