using Application.Services.Interfaces;

namespace Infrastructure.Cache
{
    public class InMemoryCacheKeyTracker : ICacheKeyTracker
    {
        private readonly HashSet<string> _keys = new();

        public void AddKey(string key)
        {
            _keys.Add(key);
        }

        public IEnumerable<string> GetKeys(string pattern)
        {
            return _keys.Where(k => k.Contains(pattern));
        }

        public void RemoveKey(string key)
        {
            _keys.Remove(key);
        }
    }
}
