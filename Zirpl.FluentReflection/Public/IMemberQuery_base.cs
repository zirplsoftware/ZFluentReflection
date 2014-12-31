using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMemberQuery<out TMemberInfo, out TMemberQuery> : IQueryResult<TMemberInfo>, ICacheableQuery<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        IMemberAccessibilitySubQuery<TMemberInfo, TMemberQuery> OfAccessibility();
        IMemberScopeSubQuery<TMemberInfo, TMemberQuery> OfScope();
    }
}