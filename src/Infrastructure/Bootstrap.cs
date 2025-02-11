using Application.Interfaces;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		AddPersistence(services, configuration);

		


		return services;
	}

	
}