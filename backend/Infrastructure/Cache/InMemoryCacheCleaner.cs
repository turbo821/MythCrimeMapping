using Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache
{
    public class InMemoryCacheCleaner : ICacheCleaner
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyTracker _keyTracker;

        public InMemoryCacheCleaner(IMemoryCache memoryCache, ICacheKeyTracker keyTracker)
        {
            _memoryCache = memoryCache;
            _keyTracker = keyTracker;
        }

        public Task RemoveByPatternAsync(string pattern)
        {
            var keys = _keyTracker.GetKeys(pattern);
            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
                _keyTracker.RemoveKey(key);
            }

            return Task.CompletedTask;
        }
    }
}
