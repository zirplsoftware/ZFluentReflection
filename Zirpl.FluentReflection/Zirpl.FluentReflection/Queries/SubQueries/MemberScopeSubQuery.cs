using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberScopeSubQuery<TMemberInfo, TReturnQuery> : SubQueryBase<TMemberInfo, TReturnQuery>,
        IMemberScopeQuery<TMemberInfo, TReturnQuery>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly MemberScopeCriteria _memberScopeCriteria;

        internal MemberScopeSubQuery(TReturnQuery returnQuery, MemberScopeCriteria memberScopeCriteria)
            :base(returnQuery)
        {
            _memberScopeCriteria = memberScopeCriteria;
        }

        IMemberScopeQuery<TMemberInfo, TReturnQuery> IMemberScopeQuery<TMemberInfo, TReturnQuery>.Instance()
        {
            _memberScopeCriteria.Instance = true;
            return this;
        }

        IMemberScopeQuery<TMemberInfo, TReturnQuery> IMemberScopeQuery<TMemberInfo, TReturnQuery>.Static()
        {
            _memberScopeCriteria.Static = true;
            return this;
        }

        IMemberScopeQuery<TMemberInfo, TReturnQuery> IMemberScopeQuery<TMemberInfo, TReturnQuery>.DeclaredOnThisType()
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            return this;
        }

        IMemberScopeQuery<TMemberInfo, TReturnQuery> IMemberScopeQuery<TMemberInfo, TReturnQuery>.DeclaredOnBaseTypes()
        {
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            return this;
        }

        IMemberScopeQuery<TMemberInfo, TReturnQuery> IMemberScopeQuery<TMemberInfo, TReturnQuery>.DeclaredOnBaseTypes(int levelsDeep)
        {
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            _memberScopeCriteria.LevelsDeep = levelsDeep;
            return this;
        }

        TReturnQuery IMemberScopeQuery<TMemberInfo, TReturnQuery>.All()
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            return _returnQuery;
        }

        TReturnQuery IMemberScopeQuery<TMemberInfo, TReturnQuery>.All(int levelsDeep)
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            _memberScopeCriteria.LevelsDeep = levelsDeep;
            return _returnQuery;
        }

        TReturnQuery IMemberScopeQuery<TMemberInfo, TReturnQuery>.Default()
        {
            return _returnQuery;
        }

        TReturnQuery IMemberScopeQuery<TMemberInfo, TReturnQuery>.And()
        {
            return _returnQuery;
        }
    }
}
