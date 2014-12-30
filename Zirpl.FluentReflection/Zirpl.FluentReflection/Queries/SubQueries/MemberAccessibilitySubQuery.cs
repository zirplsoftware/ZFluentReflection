using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberAccessibilitySubQuery<TMemberInfo, TReturnQuery> : SubQueryBase<TMemberInfo, TReturnQuery>,
        IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly MemberAccessibilityCriteria _memberAccessibilityCriteria;

        internal MemberAccessibilitySubQuery(TReturnQuery returnQuery, MemberAccessibilityCriteria memberAccessibilityCriteria)
            :base(returnQuery)
        {
            _memberAccessibilityCriteria = memberAccessibilityCriteria;
        }

        IMemberAccessibilityQuery<TMemberInfo, TReturnQuery> IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.Public()
        {
            _memberAccessibilityCriteria.Public = true;
            return this;
        }

        IMemberAccessibilityQuery<TMemberInfo, TReturnQuery> IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.Private()
        {
            _memberAccessibilityCriteria.Private = true;
            return this;
        }

        IMemberAccessibilityQuery<TMemberInfo, TReturnQuery> IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.Protected()
        {
            _memberAccessibilityCriteria.Protected = true;
            return this;
        }

        IMemberAccessibilityQuery<TMemberInfo, TReturnQuery> IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.Internal()
        {
            _memberAccessibilityCriteria.Internal = true;
            return this;
        }

        IMemberAccessibilityQuery<TMemberInfo, TReturnQuery> IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.ProtectedInternal()
        {
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return this;
        }

        TReturnQuery IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.NotPrivate()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Protected = true;
            _memberAccessibilityCriteria.Internal = true;
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return _returnQuery;
        }

        TReturnQuery IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.NotPublic()
        {
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Protected = true;
            _memberAccessibilityCriteria.Internal = true;
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return _returnQuery;
        }

        TReturnQuery IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.All()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Protected = true;
            _memberAccessibilityCriteria.Internal = true;
            _memberAccessibilityCriteria.ProtectedInternal = true;
            return _returnQuery;
        }

        TReturnQuery IMemberAccessibilityQuery<TMemberInfo, TReturnQuery>.And()
        {
            return _returnQuery;
        }
    }
}
