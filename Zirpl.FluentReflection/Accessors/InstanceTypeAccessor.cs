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

        internal PropertyInfo Property(String name)
        {
            if (!_propertyMap.ContainsKey(name))
            {
                lock (_propertyMap)
                {
                    var propertyInfo = _type.QueryProperties()
                        .Named().ExactlyIgnoreCase(name)
                        .ResultSingleOrDefault();
                    if (propertyInfo == null)
                    {
                        propertyInfo = _type.QueryProperties()
                           .OfAccessibility()
                           .NotPublic()
                           .Named().ExactlyIgnoreCase(name)
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
                               .Named().ExactlyIgnoreCase(name)
                               .ResultSingleOrDefault();   
                        }
                    }
                    _propertyMap.Add(name, propertyInfo);
                }
            }
            return _propertyMap[name];
        }

        internal FieldInfo Field(String name)
        {
            if (!_fieldMap.ContainsKey(name))
            {
                lock (_fieldMap)
                {
                    var fieldInfo = _type.QueryFields()
                        .Named().ExactlyIgnoreCase(name)
                        .ResultSingleOrDefault();
                    if (fieldInfo == null)
                    {
                        fieldInfo = _type.QueryFields()
                           .OfAccessibility()
                           .NotPublic()
                           .Named().ExactlyIgnoreCase(name)
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
                               .Named().ExactlyIgnoreCase(name)
                               .ResultSingleOrDefault();
                        }
                    }
                    _fieldMap.Add(name, fieldInfo);
                }
            }
            return _fieldMap[name];
        }

        internal EventInfo Event(String name)
        {
            if (!_eventMap.ContainsKey(name))
            {
                lock (_eventMap)
                {
                    var eventInfo = _type.QueryEvents()
                        .Named().ExactlyIgnoreCase(name)
                        .ResultSingleOrDefault();
                    if (eventInfo == null)
                    {
                        eventInfo = _type.QueryEvents()
                           .OfAccessibility()
                           .NotPublic()
                           .Named().ExactlyIgnoreCase(name)
                           .ResultSingleOrDefault();
                    }
                    if (eventInfo == null)
                    {
                        var type = _type.BaseType;
                        while (type != null && eventInfo == null)
                        {
                            eventInfo = _type.QueryEvents()
                               .OfAccessibility()
                               .Private().And()
                               .Named().ExactlyIgnoreCase(name)
                               .ResultSingleOrDefault();
                        }
                    }
                    _eventMap.Add(name, eventInfo);
                }
            }
            return _eventMap[name];
        }
    }
}
