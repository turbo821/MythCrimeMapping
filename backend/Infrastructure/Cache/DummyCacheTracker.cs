using Application.Services.Interfaces;

namespace Infrastructure.Cache
{
    public class DummyCacheTracker : ICacheKeyTracker
    {
        private readonly HashSet<string> _keys = new();

        public void AddKey(string key)
        {
            return;
        }

        public IEnumerable<string> GetKeys(string pattern)
        {
            return new List<string>();
        }

        public void RemoveKey(string key)
        {
            return;
        }
    }
}
