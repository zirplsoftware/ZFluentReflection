using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMemberScopeSubQuery<out TMemberInfo, out TReturnQuery> : IQueryResult<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> Instance();
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> Static();
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> DeclaredOnThisType();
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> DeclaredOnBaseTypes();
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> DeclaredOnBaseTypes(int levelsDeep);
        TReturnQuery All();
        TReturnQuery All(int levelsDeep);
        TReturnQuery Default();
        TReturnQuery And();
    }
}