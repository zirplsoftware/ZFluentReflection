using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMemberQuery<out TMemberInfo, out TMemberQuery> : IQueryResult<TMemberInfo>, ICacheableQuery<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        IMemberAccessibilitySubQuery<TMemberInfo, TMemberQuery> OfAccessibility();
        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TMemberQuery> OfAccessibilityCSharp();
        IMemberScopeSubQuery<TMemberInfo, TMemberQuery> OfScope();
    }
}