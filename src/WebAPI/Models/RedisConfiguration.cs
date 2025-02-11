namespace WebAPI.Models;

public class RedisConfiguration
{
	public required string ConnectionString { get; set; }
	public int CacheExpirationTimeInMinutes { get; set; }
}