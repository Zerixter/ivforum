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
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Resources.connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<UserModel>()
                .HasMany(m => m.Proyectos)
                .WithOne(m => m.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<Proyecto>()
                .HasOne(o => o.Owner)
                .WithMany(m => m.Proyectos);
            mb.Entity<Proyecto>()
                .HasMany(m => m.Users);
        }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
