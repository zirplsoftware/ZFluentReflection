using System;
using System.Collections.Generic;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class CacheService
    {
        private static IDictionary<String, Object> _cache;

        private static IDictionary<String, Object> Cache
        {
            get
            {
                if (_cache == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _cache, new Dictionary<String, Object>(), null);
                }
                return _cache;
            }
        }

        internal static void ClearCache()
        {
            Cache.Clear();
        }

        internal void Set(String key, Object obj)
        {
            Cache[key] = obj;
        }        
        internal Object Get(String key)
        {
            Object value = null;
            Cache.TryGetValue(key, out value);
            return value;
        }
    }
}
