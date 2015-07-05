using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
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
        private readonly IDictionary<String, PropertyInfo> _propertyMap;
        private readonly IDictionary<String, FieldInfo> _fieldMap;
        private readonly IDictionary<String, EventInfo> _eventMap;

        private InstanceTypeAccessor(Type type)
        {
            _type = type;
            _propertyMap = new Dictionary<string, PropertyInfo>();
            _fieldMap = new Dictionary<string, FieldInfo>();
            _eventMap = new Dictionary<string, EventInfo>();
        }

        internal PropertyInfo PropertyInfo(String name)
        {
            if (!_propertyMap.ContainsKey(name.ToLower()))
            {
                lock (_propertyMap)
                {
                    var propertyInfo = _type.QueryProperties()
                            .OfAccessibility(b => b.Public())
                            .Named(b => b.ExactlyIgnoreCase(name))
                            .ResultSingleOrDefault()
                        ?? _type.QueryProperties()
                            .OfAccessibility(b => b.NotPublic())
                            .Named(b => b.ExactlyIgnoreCase(name))
                            .ResultSingleOrDefault();
                    if (propertyInfo == null)
                    {
                        var type = _type.BaseType;
                        while (type != null && propertyInfo == null)
                        {
                            propertyInfo = type.QueryProperties()
                                .OfAccessibility(b => b.Private())
                                .Named(b => b.ExactlyIgnoreCase(name))
                               .ResultSingleOrDefault();
                            type = type.BaseType;
                        }
                    }
                    _propertyMap.Add(name.ToLower(), propertyInfo);
                }
            }
            return _propertyMap[name.ToLower()];
        }

        internal FieldInfo FieldInfo(String name)
        {
            if (!_fieldMap.ContainsKey(name.ToLower()))
            {
                lock (_fieldMap)
                {
                    var fieldInfo = _type.QueryFields()
                            .OfAccessibility(b => b.Public())
                            .Named(b => b.ExactlyIgnoreCase(name))
                            .ResultSingleOrDefault()
                        ?? _type.QueryFields()
                            .OfAccessibility(b => b.NotPublic())
                            .Named(b => b.ExactlyIgnoreCase(name))
                            .ResultSingleOrDefault();
                    if (fieldInfo == null)
                    {
                        var type = _type.BaseType;
                        while (type != null && fieldInfo == null)
                        {
                            fieldInfo = type.QueryFields()
                                .OfAccessibility(b => b.Private())
                               .Named(b => b.ExactlyIgnoreCase(name))
                               .ResultSingleOrDefault();
                            type = type.BaseType;
                        }
                    }
                    _fieldMap.Add(name.ToLower(), fieldInfo);
                }
            }
            return _fieldMap[name.ToLower()];
        }

        internal EventInfo EventInfo(String name)
        {
            if (!_eventMap.ContainsKey(name.ToLower()))
            {
                lock (_eventMap)
                {
                    var eventInfo = _type.QueryEvents()
                            .OfAccessibility(b => b.Public())
                            .Named(b => b.ExactlyIgnoreCase(name))
                            .ResultSingleOrDefault()
                        ?? _type.QueryEvents()
                            .OfAccessibility(b => b.NotPublic())
                            .Named(b => b.ExactlyIgnoreCase(name))
                            .ResultSingleOrDefault();
                    if (eventInfo == null)
                    {
                        var type = _type.BaseType;
                        while (type != null && eventInfo == null)
                        {
                            eventInfo = type.QueryEvents()
                               .OfAccessibility(b => b.Private())
                               .Named(b => b.ExactlyIgnoreCase(name))
                               .ResultSingleOrDefault();
                            type = type.BaseType;
                        }
                    }
                    _eventMap.Add(name.ToLower(), eventInfo);
                }
            }
            return _eventMap[name.ToLower()];
        }
    }
}
