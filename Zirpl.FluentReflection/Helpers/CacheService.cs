using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal sealed class CacheService
    {
        private static IDictionary<String, Object> _map;

        private static IDictionary<String, Object> Map
        {
            get
            {
                if (_map == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _map, new Dictionary<String, Object>(), null);
                }
                return _map;
            }
        }

        internal void Set(String key, Object obj)
        {
            Map[key] = obj;
        }        
        internal Object Get(String key)
        {
            Object value = null;
            Map.TryGetValue(key, out value);
            return value;
        }

        internal void Clear()
        {
            _map.Clear();
        }
    }
}
