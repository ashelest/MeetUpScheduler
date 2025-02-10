﻿namespace Infrastructure.Persistence.Redis;

public class RedisConfiguration
{
	public string ConnectionString { get; set; }
	public int CacheExpirationTimeInMinutes { get; set; }
}