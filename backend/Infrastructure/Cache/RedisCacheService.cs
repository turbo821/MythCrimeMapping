using Application.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
using IDatabase = StackExchange.Redis.IDatabase;
namespace Infrastructure.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redis;
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _redis.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(value!, _serializerOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var json = JsonSerializer.Serialize(value, _serializerOptions);
            await _redis.StringSetAsync(key, json, expiration);
        }

        public Task RemoveAsync(string key) => _redis.KeyDeleteAsync(key);

        public async Task<bool> ExistsAsync(string key) => await _redis.KeyExistsAsync(key);
    }
}
