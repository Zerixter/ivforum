using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Models
{
	public class ApplicationDbContext: IdentityDbContext
    {

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebAPI;MultipleActiveResultSets=True;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{

		}
	}
}
