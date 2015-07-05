using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
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
        ITypeQuery ITypeQuery.OfTypeCompatibility(Action<ITypeCompatibilityCriteriaBuilder> builder)
        {
            builder(new TypeCompatibilityCriteriaBuilder(_typeCriteria));
            return this;
        }

        ITypeQuery ITypeQuery.Named(Action<ITypeNameCriteriaBuilder> builder)
        {
            builder(new TypeNameCriteriaBuilder(_typeCriteria.NameCriteria));
            return this;
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
            return _typeCriteria.GetMatches(matches).Select(o => (Type)o);
        }
    }
}
