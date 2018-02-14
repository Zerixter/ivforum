using IVForum.API.Models;
using IVForum.API.Properties;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Data
{
	public class DbHandler : DbContext
    {
		public DbSet<User> Users { get; set; }
		public DbSet<Forum> Forums { get; set; }
		public DbSet<Project> Projects { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebAPI;MultipleActiveResultSets=True;Trusted_Connection=True;");
            optionsBuilder.UseMySQL(DbInfo.ConnectionString);
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<User>()
				.HasMany(x => x.Forums)
				.WithOne(x => x.Owner);

			builder.Entity<User>()
				.HasMany(x => x.Projects)
				.WithOne(x => x.Owner);

			builder.Entity<Forum>()
				.HasOne(x => x.Owner)
				.WithMany(x => x.Forums)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Project>()
				.HasOne(x => x.Owner)
				.WithMany(x => x.Projects)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
