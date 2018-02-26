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
                .HasMany(x => x.Wallets)
                .WithOne(x => x.Owner);

            builder.Entity<Forum>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Forums)
                .HasForeignKey(x => x.OwnerId)
				.OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Project>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Projects)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Project>()
                .HasOne(x => x.Forum)
                .WithMany(x => x.Projects)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Forum>()
                .HasMany(x => x.Wallets)
                .WithOne(x => x.Forum);

            builder.Entity<Forum>()
                .HasMany(x => x.Projects)
                .WithOne(x => x.Forum);

            builder.Entity<Wallet>()
                .HasOne(x => x.Forum)
                .WithMany(x => x.Wallets);

            builder.Entity<Wallet>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Wallets)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Wallet>()
                .HasMany(x => x.Bills)
                .WithOne(x => x.Wallet);

            builder.Entity<Bill>()
                .HasOne(x => x.Wallet)
                .WithMany(x => x.Bills)
                .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
