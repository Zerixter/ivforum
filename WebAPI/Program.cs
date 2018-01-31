using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace WebAPI
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
				//.UseKestrel(options => {
				//	options.Listen(IPAddress.Loopback, 5000);
				//	options.Listen(IPAddress.Loopback, 5001, listenOptions => {
				//		listenOptions.UseHttps("certificate.pfx", "password");
				//	});
				//})
                .Build();
    }
}
