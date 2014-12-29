using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal abstract class TypeMemberQueryBase<TMemberInfo, TMemberQuery> : 
        IMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo 
        where TMemberQuery : IMemberQuery<TMemberInfo, TMemberQuery>
    {
        private readonly Type _type;
        private readonly BindingFlagsBuilder _bindingFlagsBuilder;
        private readonly AccessibilityEvaluator _memberAccessibilityEvaluator;
        private readonly MemberScopeEvaluator _memberScopeEvaluator;
        private readonly MemberTypeFlagsBuilder _memberTypeFlagsBuilder;
        protected readonly MemberTypeEvaluator _memberTypeEvaluator;
        protected readonly IList<IMatchEvaluator> _matchEvaluators;
        protected readonly NameEvaluator _memberNameEvaluator;

        internal TypeMemberQueryBase(Type type)
        {
            _type = type;
            _memberScopeEvaluator = new MemberScopeEvaluator(type);
            _memberAccessibilityEvaluator = new AccessibilityEvaluator();
            _memberNameEvaluator = new NameEvaluator();
            _memberTypeEvaluator = new MemberTypeEvaluator();
            _bindingFlagsBuilder = new BindingFlagsBuilder(_memberAccessibilityEvaluator, _memberScopeEvaluator, _memberNameEvaluator);
            _memberTypeFlagsBuilder = new MemberTypeFlagsBuilder(_memberTypeEvaluator);
            _matchEvaluators = new List<IMatchEvaluator>();
            _matchEvaluators.Add(_memberNameEvaluator);
            _matchEvaluators.Add(_memberAccessibilityEvaluator);
            _matchEvaluators.Add(_memberScopeEvaluator);
            _matchEvaluators.Add(_memberTypeEvaluator);
        }

        private bool _executed;

        #region IQueryResult implementation
        IEnumerable<TMemberInfo> IQueryResult<TMemberInfo>.Execute()
        {
            if (_executed) throw new InvalidOperationException("Cannot execute twice. Use a new query.");

            _executed = true;
            var memberQueryService = new MemberQueryService(_type);
            var names = new List<String>();
            if ((_memberNameEvaluator.Name != null
                    || _memberNameEvaluator.Names != null)
                && !_memberNameEvaluator.Contains
                && !_memberNameEvaluator.EndsWith
                && !_memberNameEvaluator.StartsWith)
            {
                if (_memberNameEvaluator.Name != null)
                {
                    names.Add(_memberNameEvaluator.Name);
                }
                else
                {
                    names.AddRange(_memberNameEvaluator.Names);
                }
                _memberNameEvaluator.SkipMatchChecking = true;
            }
            var matches = memberQueryService.FindMembers(_memberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, names);
            if (_memberScopeEvaluator.DeclaredOnBaseTypes && _memberAccessibilityEvaluator.Private)
            {
                var privateMatches = memberQueryService.FindPrivateMembersOnBaseTypes(_memberTypeFlagsBuilder.MemberTypeFlags, _bindingFlagsBuilder.BindingFlags, _memberScopeEvaluator.LevelsDeep.GetValueOrDefault(), names);
                matches = matches.Union(privateMatches);
            }
            var results = from memberInfo in matches
                          where _matchEvaluators.All(eval => eval.IsMatch(memberInfo))
                          select (TMemberInfo)memberInfo;
            return results;
        }

        TMemberInfo IQueryResult<TMemberInfo>.ExecuteSingle()
        {
            var result = ((IQueryResult<TMemberInfo>)this).Execute();
            if (result.Count() > 1) throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result.Single();
        }

        TMemberInfo IQueryResult<TMemberInfo>.ExecuteSingleOrDefault()
        {
            var result = ((IQueryResult<TMemberInfo>)this).Execute();
            if (result.Count() > 1) throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result.SingleOrDefault();
        }
        #endregion


        IMemberAccessibilityQuery<TMemberInfo, TMemberQuery> IMemberQuery<TMemberInfo, TMemberQuery>.OfAccessibility()
        {
            return new MemberAccessibilitySubQuery<TMemberInfo, TMemberQuery>((TMemberQuery)(Object)this, _memberAccessibilityEvaluator);
        }

        IMemberScopeQuery<TMemberInfo, TMemberQuery> IMemberQuery<TMemberInfo, TMemberQuery>.OfScope()
        {
            return new MemberScopeSubQuery<TMemberInfo, TMemberQuery>((TMemberQuery)(Object)this, _memberScopeEvaluator);
        }
    }
}
