using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal sealed class InstanceTypeAccessor
    {
        private static IDictionary<Type, InstanceTypeAccessor> _map;

        private static IDictionary<Type, InstanceTypeAccessor> Map
        {
            get
            {
                if (_map == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _map, new Dictionary<Type, InstanceTypeAccessor>(), null);
                }
                return _map;
            }
        }

        internal static InstanceTypeAccessor Get(Type type)
        {
            if (!Map.ContainsKey(type))
            {
                lock (Map)
                {
                    if (!Map.ContainsKey(type))
                    {
                        Map.Add(type, new InstanceTypeAccessor(type));
                    }
                }
            }
            return Map[type];
        }

        internal static void ClearCache()
        {
            lock (Map)
            {
                Map.Clear();
            }
        }

        private readonly Type _type;
        private readonly IDictionary<String, PropertyInfo> _propertyAccessorMap;
        private readonly IDictionary<String, FieldInfo> _fieldAccessorMap;

        private InstanceTypeAccessor(Type type)
        {
            _type = type;
            _propertyAccessorMap = new Dictionary<string, PropertyInfo>();
            _fieldAccessorMap = new Dictionary<string, FieldInfo>();
        }

        internal PropertyInfo Property(String name)
        {
            if (!_propertyAccessorMap.ContainsKey(name))
            {
                lock (_propertyAccessorMap)
                {
                    var propertyInfo = _type.QueryProperties()
                        .Named()
                        .Exactly(name)
                        .ResultSingleOrDefault();
                    if (propertyInfo == null)
                    {
                        propertyInfo = _type.QueryProperties()
                           .OfAccessibility()
                           .NotPublic()
                           .Named()
                           .Exactly(name)
                           .ResultSingleOrDefault();
                    }
                    if (propertyInfo == null)
                    {
                        var type = _type.BaseType;
                        while (type != null && propertyInfo == null)
                        {
                            propertyInfo = _type.QueryProperties()
                               .OfAccessibility()
                               .Private().And()
                               .Named()
                               .Exactly(name)
                               .ResultSingleOrDefault();   
                        }
                    }
                    _propertyAccessorMap.Add(name, propertyInfo);
                }
            }
            return _propertyAccessorMap[name];
        }

        internal FieldInfo Field(String name)
        {
            if (!_fieldAccessorMap.ContainsKey(name))
            {
                lock (_fieldAccessorMap)
                {
                    var fieldInfo = _type.QueryFields()
                        .Named()
                        .Exactly(name)
                        .ResultSingleOrDefault();
                    if (fieldInfo == null)
                    {
                        fieldInfo = _type.QueryFields()
                           .OfAccessibility()
                           .NotPublic()
                           .Named()
                           .Exactly(name)
                           .ResultSingleOrDefault();
                    }
                    if (fieldInfo == null)
                    {
                        var type = _type.BaseType;
                        while (type != null && fieldInfo == null)
                        {
                            fieldInfo = _type.QueryFields()
                               .OfAccessibility()
                               .Private().And()
                               .Named()
                               .Exactly(name)
                               .ResultSingleOrDefault();
                        }
                    }
                    _fieldAccessorMap.Add(name, fieldInfo);
                }
            }
            return _fieldAccessorMap[name];
        }
    }
}
