using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberScopeSubQuery<TMemberInfo, TReturnQuery> : SubQueryBase<TMemberInfo, TReturnQuery>,
        IMemberScopeSubQuery<TMemberInfo, TReturnQuery>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly MemberScopeCriteria _memberScopeCriteria;

        internal MemberScopeSubQuery(TReturnQuery returnQuery, MemberScopeCriteria memberScopeCriteria)
            :base(returnQuery)
        {
            _memberScopeCriteria = memberScopeCriteria;
        }

        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.Instance()
        {
            _memberScopeCriteria.Instance = true;
            return this;
        }

        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.Static()
        {
            _memberScopeCriteria.Static = true;
            return this;
        }

        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.DeclaredOnThisType()
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            return this;
        }

        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.DeclaredOnBaseTypes()
        {
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            return this;
        }

        IMemberScopeSubQuery<TMemberInfo, TReturnQuery> IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.DeclaredOnBaseTypes(int levelsDeep)
        {
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            _memberScopeCriteria.LevelsDeep = levelsDeep;
            return this;
        }

        TReturnQuery IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.All()
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            return _returnQuery;
        }

        TReturnQuery IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.All(int levelsDeep)
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            _memberScopeCriteria.LevelsDeep = levelsDeep;
            return _returnQuery;
        }

        TReturnQuery IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.Default()
        {
            return _returnQuery;
        }

        TReturnQuery IMemberScopeSubQuery<TMemberInfo, TReturnQuery>.And()
        {
            return _returnQuery;
        }
    }
}
