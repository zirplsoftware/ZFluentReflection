using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMemberQuery<out TMemberInfo, out TMemberQuery> : IQueryResult<TMemberInfo>, ICacheableQuery<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        TMemberQuery OfAccessibility(Action<IAccessibilityCriteriaBuilder> builder);
        TMemberQuery OfScope(Action<IScopeCriteriaBuilder> builder);
    }
}