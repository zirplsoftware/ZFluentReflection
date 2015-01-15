using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.Criteria;

namespace Zirpl.FluentReflection.Queries.Implementation.SubQueries
{
    internal sealed class MemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> : SubQueryBase<TMemberInfo, TReturnQuery>,
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>,
        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly MemberAccessibilityCriteria _memberAccessibilityCriteria;

        internal MemberAccessibilitySubQuery(TReturnQuery returnQuery, MemberAccessibilityCriteria memberAccessibilityCriteria)
            :base(returnQuery)
        {
            _memberAccessibilityCriteria = memberAccessibilityCriteria;
        }

        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Public()
        {
            _memberAccessibilityCriteria.Public = true;
            return this;
        }

        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Private()
        {
            _memberAccessibilityCriteria.Private = true;
            return this;
        }

        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Protected()
        {
            _memberAccessibilityCriteria.Family = true;
            return this;
        }

        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Internal()
        {
            _memberAccessibilityCriteria.Assembly = true;
            return this;
        }

        ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> ICSharpMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.ProtectedOrInternal()
        {
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
            return this;
        }

        public TReturnQuery NotPrivate()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Family = true;
            _memberAccessibilityCriteria.Assembly = true;
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
            return _returnQuery;
        }

        public TReturnQuery NotPublic()
        {
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Family = true;
            _memberAccessibilityCriteria.Assembly = true;
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
            return _returnQuery;
        }

        public TReturnQuery All()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Family = true;
            _memberAccessibilityCriteria.Assembly = true;
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
            return _returnQuery;
        }

        public TReturnQuery And()
        {
            return _returnQuery;
        }

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Public()
        {
            _memberAccessibilityCriteria.Public = true;
            return this;
        }

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Private()
        {
            _memberAccessibilityCriteria.Private = true;
            return this;
        }

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Family()
        {
            _memberAccessibilityCriteria.Family = true;
            return this;
        }

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Assembly()
        {
            _memberAccessibilityCriteria.Assembly = true;
            return this;
        }

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.FamilyOrAssembly()
        {
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
            return this;
        }
    }
}
