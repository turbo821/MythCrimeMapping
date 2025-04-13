using Application.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public class RedisCacheCleaner : ICacheCleaner
    {
        private readonly IDatabase _cache;
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheCleaner(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _cache = redis.GetDatabase();
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoints.First());

            var keys = server.Keys(pattern: $"*{pattern}*").ToArray();

            if (keys.Length > 0)
            {
                await _cache.KeyDeleteAsync(keys);
            }
        }
    }
}
