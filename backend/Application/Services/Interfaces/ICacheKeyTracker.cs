using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICacheKeyTracker
    {
        void AddKey(string key);
        IEnumerable<string> GetKeys(string pattern);
        void RemoveKey(string key);
    }
}
