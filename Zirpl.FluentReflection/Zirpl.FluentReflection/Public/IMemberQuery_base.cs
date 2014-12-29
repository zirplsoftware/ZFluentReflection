using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMemberQuery<out TMemberInfo, out TMemberQuery> : IQueryResult<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        IMemberAccessibilityQuery<TMemberInfo, TMemberQuery> OfAccessibility();
        IMemberScopeQuery<TMemberInfo, TMemberQuery> OfScope();
    }
}