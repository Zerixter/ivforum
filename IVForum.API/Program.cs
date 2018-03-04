using IVForum.API.Data;
using IVForum.API.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IVForum.API
{
	public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:80/")
                .Build();
    }
}
