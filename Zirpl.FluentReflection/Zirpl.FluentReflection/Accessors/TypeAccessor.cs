using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal sealed class TypeAccessor
    {
        private static IDictionary<Type, TypeAccessor> _map;

        private static IDictionary<Type, TypeAccessor> Map
        {
            get
            {
                if (_map == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _map, new Dictionary<Type, TypeAccessor>(), null);
                }
                return _map;
            }
        }

        internal static TypeAccessor Get(Type type)
        {
            if (!Map.ContainsKey(type))
            {
                lock (Map)
                {
                    if (!Map.ContainsKey(type))
                    {
                        Map.Add(type, new TypeAccessor(type));
                    }
                }
            }
            return Map[type];
        }

        private readonly Type _type;
        private readonly IDictionary<String, PropertyAccessor> _propertyAccessorMap;
        private readonly IDictionary<String, FieldAccessor> _fieldAccessorMap;

        private TypeAccessor(Type type)
        {
            _type = type;
            _propertyAccessorMap = new Dictionary<string, PropertyAccessor>();
            _fieldAccessorMap = new Dictionary<string, FieldAccessor>();
        }

        public PropertyAccessor Property(String name)
        {
            if (!_propertyAccessorMap.ContainsKey(name))
            {
                lock (_propertyAccessorMap)
                {
                    var propertyInfo = _type.QueryProperties()
                        .Named()
                        .Exactly(name)
                        .ExecuteSingleOrDefault();
                    if (propertyInfo == null)
                    {
                        propertyInfo = _type.QueryProperties()
                           .OfAccessibility()
                           .NotPublic()
                           .Named()
                           .Exactly(name)
                           .ExecuteSingleOrDefault();
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
                               .ExecuteSingleOrDefault();   
                        }
                    }
                    _propertyAccessorMap.Add(name, new PropertyAccessor(propertyInfo));
                }
            }
            return _propertyAccessorMap[name];
        }

        public FieldAccessor Field(String name)
        {
            if (!_fieldAccessorMap.ContainsKey(name))
            {
                lock (_fieldAccessorMap)
                {
                    var fieldInfo = _type.QueryFields()
                        .Named()
                        .Exactly(name)
                        .ExecuteSingleOrDefault();
                    if (fieldInfo == null)
                    {
                        fieldInfo = _type.QueryFields()
                           .OfAccessibility()
                           .NotPublic()
                           .Named()
                           .Exactly(name)
                           .ExecuteSingleOrDefault();
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
                               .ExecuteSingleOrDefault();
                        }
                    }
                    _fieldAccessorMap.Add(name, new FieldAccessor(fieldInfo));
                }
            }
            return _fieldAccessorMap[name];
        }
    }
}
