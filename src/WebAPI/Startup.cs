using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAPI.Converters;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Persistence;
using WebAPI.Services;

namespace WebAPI;

public class Startup(IConfiguration configuration)
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				options.JsonSerializerOptions.Converters.Add(new JsonDateTimeOffsetConverter());
			});

		services.AddExceptionHandler<CustomExceptionHandler>();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		AddPersistence(services);
		AddCaching(services);
		AddServices(services);
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		// Configure the HTTP request pipeline.
		if (env.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseExceptionHandler(options => { });
		app.UseHttpsRedirection();

		app.UseRouting();
		app.UseAuthorization();

		app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
	}

	private void AddServices(IServiceCollection services)
	{
		services.AddScoped<IMeetUpSchedulerService, MeetUpSchedulerService>();
		services.Decorate<IMeetUpSchedulerService, CachedMeetUpSchedulerService>();

		services.AddScoped<ICachingService, CachingService>();
	}

	private void AddPersistence(IServiceCollection services)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");

		services.AddDbContextPool<IApplicationDbContext, ApplicationDbContext>(opt => opt.UseNpgsql(connectionString));
	}

	private void AddCaching(IServiceCollection services)
	{
		services.Configure<RedisConfiguration>(configuration.GetSection(nameof(RedisConfiguration)));

		using var serviceProvider = services.BuildServiceProvider();
		var redisConfiguration = serviceProvider.GetRequiredService<IOptions<RedisConfiguration>>().Value;

		services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = redisConfiguration.ConnectionString;
			options.InstanceName = "MeetUpScheduler";
		});
	}
}