using Application.Services.Interfaces;
using Infrastructure.Cache;
using StackExchange.Redis;

namespace Web
{
    public static class WebAppExtensions
    {
        public static IServiceCollection AddCacheWithFallback(this IServiceCollection services, IConfiguration configuration)
        {
            var redisAvailable = false;

            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? configuration.GetSection("Redis:ConnectionString").Value!;

            try
            {
                var redis = ConnectionMultiplexer.Connect(redisHost);
                if (redis.IsConnected)
                {
                    services.AddSingleton<IConnectionMultiplexer>(redis);
                    services.AddSingleton<ICacheService, RedisCacheService>();
                    services.AddSingleton<ICacheCleaner, RedisCacheCleaner>();
                    services.AddSingleton<ICacheKeyTracker, DummyCacheTracker>();
                    redisAvailable = true;
                }
            }
            catch { }

            if (!redisAvailable)
            {
                services.AddMemoryCache();
                services.AddSingleton<ICacheService, InMemoryCacheService>();
                services.AddSingleton<ICacheCleaner, InMemoryCacheCleaner>();
                services.AddSingleton<ICacheKeyTracker, InMemoryCacheKeyTracker>();
            }

            return services;
        }
    }
}
