using Microsoft.AspNetCore;

namespace WebAPI;

public class Program
{
	public static void Main(string[] args)
	{
		var webHost = CreateWebHostBuilder(args).Build();

		webHost.Run();
	}

	private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
		WebHost.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((ctx, builder) =>
			{
				builder.AddJsonFile("appsettings.json", false, true);
				builder.AddEnvironmentVariables();
			})
			.UseStartup<Startup>();
}