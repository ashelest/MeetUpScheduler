using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Services;

public class CachingService(IOptions<RedisConfiguration> options, IDistributedCache cache) : ICachingService
{
	private readonly TimeSpan cacheExpirationTime = TimeSpan.FromMinutes(options.Value.CacheExpirationTimeInMinutes);

	public async Task SetAsync<TKey, TValue>(TKey key, TValue value)
	{
		await cache.SetStringAsync(Serialize<TKey>(key), Serialize<TValue>(value), new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = cacheExpirationTime
		});
	}

	public async Task<TValue?> GetAsync<TKey, TValue>(TKey key) where TValue : class
	{
		var cachedData = await cache.GetStringAsync(Serialize<TKey>(key));

		return string.IsNullOrWhiteSpace(cachedData) ? default : JsonSerializer.Deserialize<TValue>(cachedData);
	}

	private static string Serialize<T>(T source)
	{
		return JsonSerializer.Serialize(source);
	}
}