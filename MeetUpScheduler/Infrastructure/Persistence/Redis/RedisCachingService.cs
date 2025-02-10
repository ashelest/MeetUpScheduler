using System.Text.Json;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infrastructure.Persistence.Redis;

public class RedisCachingService : ICachingService
{
	private readonly IDatabase database;
	private readonly TimeSpan cacheExpirationTime;

	public RedisCachingService(IOptions<RedisConfiguration> options)
	{
		var configuration = options.Value;

		IConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(configuration.ConnectionString);

		database = redisConnection.GetDatabase();

		cacheExpirationTime = TimeSpan.FromMinutes(options.Value.CacheExpirationTimeInMinutes);
	}

	public async Task SetAsync<TKey, TValue>(TKey key, TValue value)
	{
		await database.StringSetAsync(Serialize<TKey>(key), Serialize<TValue>(value), cacheExpirationTime);
	}

	public async Task<TValue?> GetAsync<TKey, TValue>(TKey key) where TValue : class
	{
		var cachedData = await database.StringGetAsync(Serialize<TKey>(key));

		return cachedData.IsNullOrEmpty ? default : JsonSerializer.Deserialize<TValue>(cachedData);
	}

	private static string Serialize<T>(T source)
	{
		return JsonSerializer.Serialize(source);
	}
}