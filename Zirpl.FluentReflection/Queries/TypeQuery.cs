using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class TypeQuery : CacheableQueryBase<Type>, ITypeQuery
    {
        private readonly IList<Assembly> _assemblyList;
        private readonly TypeCriteria _typeCriteria;

        internal TypeQuery(Assembly assembly)
        {
            _assemblyList = new List<Assembly>();
            _assemblyList.Add(assembly);
            _typeCriteria = new TypeCriteria(TypeSource.Self);
        }
#if !PORTABLE
        internal TypeQuery(AppDomain appDomain)
        {
            foreach (var assembly in appDomain.GetAssemblies())
            {
                _assemblyList.Add(assembly);
            }
            _typeCriteria = new TypeCriteria(TypeSource.Self);
        }
#endif

        ITypeQuery ITypeQuery.AssignableFrom(Type type)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = new[] { type };
            return this;
        }

        ITypeQuery ITypeQuery.AssignableFrom<T>()
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = new[] { typeof(T) };
            return this;
        }

        ITypeQuery ITypeQuery.AssignableFromAll(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = types;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableFromAny(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = types;
            _typeCriteria.Any = true;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableTo(Type type)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = new[] { type };
            return this;
        }

        ITypeQuery ITypeQuery.AssignableTo<T>()
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = new[] { typeof(T) };
            return this;
        }

        ITypeQuery ITypeQuery.AssignableToAll(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = types;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableToAny(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = types;
            _typeCriteria.Any = true;
            return this;
        }

        ITypeNameSubQuery<Type, ITypeQuery> ITypeQuery.Named()
        {
            return new TypeNameSubQuery<Type, ITypeQuery>(this, _typeCriteria.NameCriteria);
        }

        protected override string CacheKeyPrefix
        {
            get { return _assemblyList.Count > 1 ? "appdomain" : _assemblyList[0].FullName; }
        }

        protected override IEnumerable<Type> ExecuteQuery()
        {
            var matches = (from assembly in _assemblyList.Distinct()
                           from type in assembly.GetTypes()
                           select (MemberInfo)type).ToArray();
            return _typeCriteria.FilterMatches(matches).Select(o => (Type)o);
        }
    }
}
