using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMemberAccessibilitySubQuery<out TMemberInfo, out TReturnQuery> : IQueryResult<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Public();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Private();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Family();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> Assembly();
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> FamilyOrAssembly();
        TReturnQuery NotPrivate();
        TReturnQuery NotPublic();
        TReturnQuery All();
        TReturnQuery And();
    }
}