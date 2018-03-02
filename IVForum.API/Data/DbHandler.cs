using IVForum.API.Models;
using IVForum.API.Properties;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IVForum.API.Data
{
	public class DbHandler : IdentityDbContext<UserModel>
    {
		public DbSet<User> DbUsers { get; set; }
		public DbSet<Forum> Forums { get; set; }
		public DbSet<Project> Projects { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BillOption> BillOptions { get; set; }
        public DbSet<Vote> Votes { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(DbInfo.ConnectionString);
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
            base.OnModelCreating(builder);

            #region User
            builder.Entity<User>()
                    .HasMany(x => x.Forums)
                    .WithOne(x => x.Owner)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>()
                .HasMany(x => x.Projects)
                .WithOne(x => x.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>()
                .HasMany(x => x.Wallets);
            #endregion
            #region Forum
            builder.Entity<Forum>()
                    .HasOne(x => x.Owner)
                    .WithMany(x => x.Forums);
            builder.Entity<Forum>()
                .HasMany(x => x.Wallets)
                .WithOne(x => x.Forum);
            builder.Entity<Forum>()
                .HasMany(x => x.Transactions)
                .WithOne(x => x.Forum);
            builder.Entity<Forum>()
                .HasMany(x => x.Projects)
                .WithOne(x => x.Forum)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
            #region Project
            builder.Entity<Project>()
                    .HasOne(x => x.Forum)
                    .WithMany(x => x.Projects);
            builder.Entity<Project>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Projects);
            builder.Entity<Project>()
                .HasMany(x => x.Votes)
                .WithOne(x => x.Project); 
            #endregion
            #region Wallet
            builder.Entity<Wallet>()
                .HasOne(x => x.Forum)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.ForumId);
            builder.Entity<Wallet>()
                .HasMany(x => x.Bills)
                .WithOne(x => x.Wallet);
            builder.Entity<Wallet>()
                .HasOne(x => x.User)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.UserId);
            #endregion
            #region BillOption
            builder.Entity<BillOption>()
                .HasOne(x => x.Wallet)
                .WithMany(x => x.Bills)
                .HasForeignKey(x => x.WalletId);
            #endregion
            #region Transaction
            builder.Entity<Transaction>()
                .HasOne(x => x.Forum)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.ForumId);
            #endregion
            #region Vote
            builder.Entity<Vote>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
