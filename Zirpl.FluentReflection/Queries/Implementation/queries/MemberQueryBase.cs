using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal abstract class MemberQueryBase<TMemberInfo, TMemberQuery> : CacheableQueryBase<TMemberInfo>,
        IMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo 
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        private readonly Type _type;
        private readonly BindingFlagsBuilder _bindingFlagsBuilder;
        private readonly AccessibilityCriteria _memberAccessibilityCriteria;
        private readonly ScopeCriteria _memberScopeCriteria;
        protected readonly MemberTypeFlagsBuilder MemberTypeFlagsBuilder;
        protected readonly IList<IMemberInfoCriteria> QueryCriteriaList;
        protected readonly MemberNameCriteria MemberNameCriteria;

        internal MemberQueryBase(Type type)
        {
            _type = type;
            _memberScopeCriteria = new ScopeCriteria(type);
            _memberAccessibilityCriteria = new AccessibilityCriteria();
            MemberNameCriteria = new MemberNameCriteria();
            _bindingFlagsBuilder = new BindingFlagsBuilder(_memberAccessibilityCriteria, _memberScopeCriteria, MemberNameCriteria);
            MemberTypeFlagsBuilder = new MemberTypeFlagsBuilder();
            QueryCriteriaList = new List<IMemberInfoCriteria>
            {
                MemberNameCriteria,
                _memberAccessibilityCriteria,
                _memberScopeCriteria
            };
        }

        protected override string CacheKeyPrefix
        {
            // example: for COnstructors this would return: {typeFullName}|ConstructorQuery
            get { return _type.FullName + "|" + this.GetType().Name; }
        }

        protected override IEnumerable<TMemberInfo> ExecuteQuery()
        {
            var memberQueryService = new MemberQueryService(_type);
            var names = MemberNameCriteria.GetNamesForDirectLookup();
            var matches = memberQueryService.FindMembers(MemberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, names);
            if (_memberScopeCriteria.DeclaredOnBaseTypes && _memberAccessibilityCriteria.Private)
            {
                var privateMatches = memberQueryService.FindPrivateMembersOnBaseTypes(MemberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, names);
                matches = matches.Union(privateMatches).ToArray();
            }
            matches = QueryCriteriaList.Aggregate(matches, (current, memberInfoQueryCriteria) => memberInfoQueryCriteria.GetMatches(current));
            var final = matches.Select(o => (TMemberInfo)o);
            return final;
        }

        TMemberQuery IMemberQuery<TMemberInfo, TMemberQuery>.OfAccessibility(Action<IAccessibilityCriteriaBuilder> builder)
        {
            builder(new AccessibilityCriteriaBuilder(_memberAccessibilityCriteria));
            return (TMemberQuery)(Object)this;
        }

        TMemberQuery IMemberQuery<TMemberInfo, TMemberQuery>.OfScope(Action<IScopeCriteriaBuilder> builder)
        {
            builder(new ScopeCriteriaBuilder(_memberScopeCriteria));
            return (TMemberQuery) (Object) this;
        }
    }
}
