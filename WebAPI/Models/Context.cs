using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Properties;

namespace WebAPI.Models
{
    public class Context: DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Resources.connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>()
                .HasMany(m => m.Projects)
                .WithOne(m => m.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<Project>()
                .HasOne(o => o.Owner)
                .WithMany(m => m.Projects);
            mb.Entity<Project>()
                .HasMany(m => m.Users);
        }
    }
}
