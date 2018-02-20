using IVForum.API.Models;
using IVForum.API.Properties;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Data
{
	public class DbHandler : IdentityDbContext<UserModel>
    {
		public DbSet<User> DbUsers { get; set; }
		public DbSet<Forum> Forums { get; set; }
		public DbSet<Project> Projects { get; set; }
        public DbSet<Token> Tokens { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(DbInfo.ConnectionString);
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
            base.OnModelCreating(builder);

			builder.Entity<User>()
				.HasMany(x => x.Forums)
				.WithOne(x => x.Owner);

			builder.Entity<User>()
				.HasMany(x => x.Projects)
				.WithOne(x => x.Owner);

            builder.Entity<User>()
                .HasOne(x => x.Token)
                .WithOne(x => x.User);

            builder.Entity<Token>()
                .HasOne(x => x.User)
                .WithOne(x => x.Token)
                .OnDelete(DeleteBehavior.Cascade);

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
