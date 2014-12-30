using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> : SubQueryBase<TMemberInfo, TReturnQuery>,
        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly MemberAccessibilityCriteria _memberAccessibilityCriteria;

        internal MemberAccessibilitySubQuery(TReturnQuery returnQuery, MemberAccessibilityCriteria memberAccessibilityCriteria)
            :base(returnQuery)
        {
            _memberAccessibilityCriteria = memberAccessibilityCriteria;
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

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Protected()
        {
            _memberAccessibilityCriteria.Protected = true;
            return this;
        }

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.Internal()
        {
            _memberAccessibilityCriteria.Internal = true;
            return this;
        }

        IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.ProtectedInternal()
        {
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return this;
        }

        TReturnQuery IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.NotPrivate()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Protected = true;
            _memberAccessibilityCriteria.Internal = true;
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return _returnQuery;
        }

        TReturnQuery IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.NotPublic()
        {
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Protected = true;
            _memberAccessibilityCriteria.Internal = true;
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return _returnQuery;
        }

        TReturnQuery IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.All()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Protected = true;
            _memberAccessibilityCriteria.Internal = true;
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return _returnQuery;
        }

        TReturnQuery IMemberAccessibilitySubQuery<TMemberInfo, TReturnQuery>.And()
        {
            return _returnQuery;
        }
    }
}
