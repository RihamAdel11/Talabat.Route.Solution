﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Services.CacheService
{
	public class ResponseCacheService : IResponseCacheService
	{
		private readonly IDatabase _database;
		public ResponseCacheService(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}
		public async Task CacheResponseAsync(string key, object Response, TimeSpan timeToLive)
		{
			if (Response is null) return;

			var serializedOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			var serializedResponse = JsonSerializer.Serialize(Response);

			await _database.StringSetAsync(key, serializedResponse, timeToLive);
		}

		public async Task<string?> GetCacheResponseAsync(string key)
		{
			var response = await _database.StringGetAsync(key);

			if (response.IsNullOrEmpty) return null;

			return response;
		}
	}
}
