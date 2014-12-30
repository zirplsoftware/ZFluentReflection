using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMemberAccessibilitySubQuery<out TMemberInfo, out TReturnQuery> : IQueryResult<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Public();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Private();
        //IMemberAccessibilityQuery<TMemberInfo, TReturnQuery> PrivateIncludingOnBaseTypes();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Protected();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Internal();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> ProtectedInternal();
        TReturnQuery NotPrivate();
        TReturnQuery NotPublic();
        TReturnQuery All();
        TReturnQuery And();
    }
}