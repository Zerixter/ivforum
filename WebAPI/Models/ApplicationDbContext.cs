using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebAPI.Properties;

namespace WebAPI.Models
{
	public class ApplicationDbContext: IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Resources.connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
