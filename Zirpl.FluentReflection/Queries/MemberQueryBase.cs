using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal abstract class MemberQueryBase<TMemberInfo, TMemberQuery> : 
        IMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo 
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        private readonly Type _type;
        private readonly BindingFlagsBuilder _bindingFlagsBuilder;
        private readonly MemberAccessibilityCriteria _memberAccessibilityCriteria;
        private readonly MemberScopeCriteria _memberScopeCriteria;
        private readonly MemberTypeFlagsBuilder _memberTypeFlagsBuilder;
        protected readonly MemberTypeCriteria _memberTypeCriteria;
        protected readonly IList<IMemberInfoQueryCriteria> _queryCriteriaList;
        protected readonly MemberNameCriteria _memberNameCriteria;

        internal MemberQueryBase(Type type)
        {
            _type = type;
            _memberScopeCriteria = new MemberScopeCriteria(type);
            _memberAccessibilityCriteria = new MemberAccessibilityCriteria();
            _memberNameCriteria = new MemberNameCriteria();
            _memberTypeCriteria = new MemberTypeCriteria();
            _bindingFlagsBuilder = new BindingFlagsBuilder(_memberAccessibilityCriteria, _memberScopeCriteria, _memberNameCriteria);
            _memberTypeFlagsBuilder = new MemberTypeFlagsBuilder(_memberTypeCriteria);
            _queryCriteriaList = new List<IMemberInfoQueryCriteria>();
            _queryCriteriaList.Add(_memberNameCriteria);
            _queryCriteriaList.Add(_memberAccessibilityCriteria);
            _queryCriteriaList.Add(_memberScopeCriteria);
            _queryCriteriaList.Add(_memberTypeCriteria);
        }

        private bool _executed;

        #region IQueryResult implementation
        IEnumerable<TMemberInfo> IQueryResult<TMemberInfo>.Result()
        {
            if (_executed) throw new InvalidOperationException("Cannot execute twice. Use a new query.");

            _executed = true;
            var memberQueryService = new MemberQueryService(_type);
            var names = _memberNameCriteria.GetNamesForDirectLookup();
            var matches = memberQueryService.FindMembers(_memberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, names);
            if (_memberScopeCriteria.DeclaredOnBaseTypes && _memberAccessibilityCriteria.Private)
            {
                var privateMatches = memberQueryService.FindPrivateMembersOnBaseTypes(_memberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, _memberScopeCriteria.LevelsDeep.GetValueOrDefault(), names);
                matches = matches.Union(privateMatches).ToArray();
            }
            foreach (var memberInfoQueryCriteria in _queryCriteriaList)
            {
                matches = memberInfoQueryCriteria.FilterMatches(matches);
            }
            return matches.Select(o => (TMemberInfo)o);
        }

        TMemberInfo IQueryResult<TMemberInfo>.ResultSingle()
        {
            var result = ((IQueryResult<TMemberInfo>)this).Result().ToList();
            if (result.Count() > 1) throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result[0];
        }

        TMemberInfo IQueryResult<TMemberInfo>.ResultSingleOrDefault()
        {
            var result = ((IQueryResult<TMemberInfo>)this).Result().ToList();
            if (result.Count() > 1) throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result.SingleOrDefault();
        }
        #endregion


        IMemberAccessibilitySubQuery<TMemberInfo, TMemberQuery> IMemberQuery<TMemberInfo, TMemberQuery>.OfAccessibility()
        {
            return new MemberAccessibilitySubQuery<TMemberInfo, TMemberQuery>((TMemberQuery)(Object)this, _memberAccessibilityCriteria);
        }

        IMemberScopeSubQuery<TMemberInfo, TMemberQuery> IMemberQuery<TMemberInfo, TMemberQuery>.OfScope()
        {
            return new MemberScopeSubQuery<TMemberInfo, TMemberQuery>((TMemberQuery)(Object)this, _memberScopeCriteria);
        }
    }
}
