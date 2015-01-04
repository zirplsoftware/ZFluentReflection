using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface ICSharpMemberAccessibilitySubQuery<out TMemberInfo, out TReturnQuery> : IQueryResult<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Public();
        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Private();
        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Protected();
        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Internal();
        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> ProtectedOrInternal();
        TReturnQuery NotPrivate();
        TReturnQuery NotPublic();
        TReturnQuery All();
        TReturnQuery And();
    }
}