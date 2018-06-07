using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benlai.Zookeeper.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key) where TKey : class
        {
            TValue result;
            return self.TryGetValue(key, out result) ? result : default(TValue);
        }
    }

}
