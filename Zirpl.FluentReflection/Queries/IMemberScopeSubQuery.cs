using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMemberScopeSubQuery<out TMemberInfo, out TReturnQuery> : IQueryResult<TMemberInfo>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> Instance();
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> Static();
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> DeclaredOnThisType();
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> DeclaredOnBaseTypes();
        TReturnQuery All();
        TReturnQuery Default();
        TReturnQuery And();
    }
}