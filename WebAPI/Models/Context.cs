﻿using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        private void addProjectsToDatabase()
        {
            if (!Proyectos.Any())
            {

            }
        }

        public DbSet<Proyecto> Proyectos { get; set; }
    }
}