using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class AssemblyTypeQuery : ITypeQuery
    {
        private readonly IList<Assembly> _assemblyList;
        private readonly TypeCriteria _typeCriteria;

        internal AssemblyTypeQuery(Assembly assembly)
        {
            _assemblyList = new List<Assembly>();
            _assemblyList.Add(assembly);
            _typeCriteria = new TypeCriteria();
        }
#if !PORTABLE
        internal AssemblyTypeQuery(AppDomain appDomain)
        {
            foreach (var assembly in appDomain.GetAssemblies())
            {
                _assemblyList.Add(assembly);
            }
            _typeCriteria = new TypeCriteria();
        }
#endif

        #region IQueryResult implementation
        
        IEnumerable<Type> IQueryResult<Type>.Result()
        {
            var list = new List<Type>();
            var matches = from assembly in _assemblyList.Distinct()
                from type in assembly.GetTypes()
                where _typeCriteria.IsMatch(type)
                select type;
            list.AddRange(matches);
            return list;
        }

        Type IQueryResult<Type>.ResultSingle()
        {
            var result = ((IQueryResult<Type>)this).Result().ToList();
            if (result.Count() > 1) throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result.Single();
        }

        Type IQueryResult<Type>.ResultSingleOrDefault()
        {
            var result = ((IQueryResult<Type>)this).Result().ToList();
            if (result.Count() > 1) throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result.SingleOrDefault();
        }

        #endregion

        ITypeQuery ITypeQuery.AssignableFrom(Type type)
        {
            _typeCriteria.AssignableFrom = type;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableFrom<T>()
        {
            _typeCriteria.AssignableFrom = typeof(T);
            return this;
        }

        ITypeQuery ITypeQuery.AssignableFromAll(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableFroms = types;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableFromAny(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableFroms = types;
            _typeCriteria.Any = true;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableTo(Type type)
        {
            _typeCriteria.AssignableTo = type;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableTo<T>()
        {
            _typeCriteria.AssignableTo = typeof(T);
            return this;
        }

        ITypeQuery ITypeQuery.AssignableToAll(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableTos = types;
            return this;
        }

        ITypeQuery ITypeQuery.AssignableToAny(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableTos = types;
            _typeCriteria.Any = true;
            return this;
        }

        INameSubQuery<Type, ITypeQuery> ITypeQuery.Named()
        {
            return new NameSubQuery<Type, ITypeQuery>(this, _typeCriteria.NameCriteria);
        }

        INameSubQuery<Type, ITypeQuery> ITypeQuery.FullNamed()
        {
            return new NameSubQuery<Type, ITypeQuery>(this, _typeCriteria.FullNameCriteria);
        }
    }
}
