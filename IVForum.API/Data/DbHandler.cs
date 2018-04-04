using IVForum.API.Models;
using IVForum.API.Properties;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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

        public DbHandler(DbContextOptions<DbHandler> options) : base(options) { }
        private class ConnectionStringMember
        {
            public string ConnectionString { get; set; }
        }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=webapi;MultipleActiveResultSets=True;Trusted_Connection=True;";
            try
            {
                using (StreamReader r = new StreamReader("ConnectionString.txt"))
                {
                    string text = r.ReadToEnd();
                    ConnectionString = text;
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("No s'ha trobat el fitxer ConnectionString.txt en la carpeta arrel");
            }
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(ConnectionString);
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
            base.OnModelCreating(builder);

            #region User
            builder.Entity<User>()
                    .HasMany(x => x.Forums)
                    .WithOne(x => x.Owner);
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
                .OnDelete(DeleteBehavior.SetNull);
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
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion
            #region Transaction
            builder.Entity<Transaction>()
                .HasOne(x => x.Forum)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.ForumId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region Vote
            builder.Entity<Vote>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.SetNull); ;
            #endregion
        }
    }
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<DbHandler>
    {
        public DbHandler CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbHandler>();

            return new DbHandler(optionsBuilder.Options);
        }
    }
}
