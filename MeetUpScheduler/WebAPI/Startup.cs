using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Redis;
using WebAPI.Converters;

namespace WebAPI;

public class Startup(IConfiguration configuration)
{
	public void ConfigureServices(IServiceCollection services)
	{
		// Add services to the container.

		services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				options.JsonSerializerOptions.Converters.Add(new JsonDateTimeOffsetConverter());

			});

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		services.AddApplication();
		services.AddInfrastructure(configuration);
		services.Configure<RedisConfiguration>(configuration.GetSection(nameof(RedisConfiguration)));

		services.AddTransient<ExceptionHandlingMiddleware>();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		// Configure the HTTP request pipeline.
		if (env.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseMiddleware<ExceptionHandlingMiddleware>();
		
		app.UseRouting();

		app.UseAuthorization();

		app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
	}
}