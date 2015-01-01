using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal abstract class MemberQueryBase<TMemberInfo, TMemberQuery> : CacheableQueryBase<TMemberInfo>,
        IMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo 
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        private readonly Type _type;
        private readonly BindingFlagsBuilder _bindingFlagsBuilder;
        private readonly MemberAccessibilityCriteria _memberAccessibilityCriteria;
        private readonly MemberScopeCriteria _memberScopeCriteria;
        protected readonly MemberTypeFlagsBuilder _memberTypeFlagsBuilder;
        protected readonly IList<IMemberInfoQueryCriteria> _queryCriteriaList;
        protected readonly MemberNameCriteria _memberNameCriteria;

        internal MemberQueryBase(Type type)
        {
            _type = type;
            _memberScopeCriteria = new MemberScopeCriteria(type);
            _memberAccessibilityCriteria = new MemberAccessibilityCriteria();
            _memberNameCriteria = new MemberNameCriteria();
            _bindingFlagsBuilder = new BindingFlagsBuilder(_memberAccessibilityCriteria, _memberScopeCriteria, _memberNameCriteria);
            _memberTypeFlagsBuilder = new MemberTypeFlagsBuilder();
            _queryCriteriaList = new List<IMemberInfoQueryCriteria>();
            _queryCriteriaList.Add(_memberNameCriteria);
            _queryCriteriaList.Add(_memberAccessibilityCriteria);
            _queryCriteriaList.Add(_memberScopeCriteria);
        }

        protected override string CacheKeyPrefix
        {
            get { return _type.FullName + "|" + this.GetType().Name; }
        }

        protected override IEnumerable<TMemberInfo> ExecuteQuery()
        {
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
            var final = matches.Select(o => (TMemberInfo)o);
            return final;
        }

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
