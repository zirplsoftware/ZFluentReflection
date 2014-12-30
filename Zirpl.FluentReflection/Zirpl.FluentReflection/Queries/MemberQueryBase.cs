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
        protected readonly IList<IMatchEvaluator> _matchEvaluators;
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
            _matchEvaluators = new List<IMatchEvaluator>();
            _matchEvaluators.Add(_memberNameCriteria);
            _matchEvaluators.Add(_memberAccessibilityCriteria);
            _matchEvaluators.Add(_memberScopeCriteria);
            _matchEvaluators.Add(_memberTypeCriteria);
        }

        private bool _executed;

        #region IQueryResult implementation
        IEnumerable<TMemberInfo> IQueryResult<TMemberInfo>.Result()
        {
            if (_executed) throw new InvalidOperationException("Cannot execute twice. Use a new query.");

            _executed = true;
            var memberQueryService = new MemberQueryService(_type);
            var names = new List<String>();
            if (!_memberNameCriteria.IsMatchCheckRequired())
            {
                // if the name check is not required, it implicitly means we should use any names here
                if (_memberNameCriteria.Name != null)
                {
                    names.Add(_memberNameCriteria.Name);
                }
                else if (_memberNameCriteria.Names != null)
                {
                    names.AddRange(_memberNameCriteria.Names);
                }
            }
            var matches = memberQueryService.FindMembers(_memberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, names);
            if (_memberScopeCriteria.DeclaredOnBaseTypes && _memberAccessibilityCriteria.Private)
            {
                var privateMatches = memberQueryService.FindPrivateMembersOnBaseTypes(_memberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, _memberScopeCriteria.LevelsDeep.GetValueOrDefault(), names);
                matches = matches.Union(privateMatches);
            }
            var evaluatorsToUse = _matchEvaluators.Where(eval => eval.IsMatchCheckRequired()).ToList();
            if (evaluatorsToUse.Any())
            {
                return from memberInfo in matches.Distinct()
                          where evaluatorsToUse.All(eval => eval.IsMatch(memberInfo))
                          select (TMemberInfo)memberInfo;
            }
            else
            {
                return from memberInfo in matches.Distinct()
                          select (TMemberInfo)memberInfo;
            }
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
