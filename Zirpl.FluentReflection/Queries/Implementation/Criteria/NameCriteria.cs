using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries.Implementation.Criteria
{
    internal abstract class NameCriteria : MemberInfoQueryCriteriaBase
    {
        private IEnumerable<String> _names;

        internal NameHandlingType NameHandling { get; set; }
        internal bool IgnoreCase { get; set; }
        internal IEnumerable<String> Names
        {
            get { return _names; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (!value.Any()) throw new ArgumentException("Must have at least one entry", "value");
                if (value.Any(String.IsNullOrEmpty)) throw new ArgumentException("An entry is null", "value");

                _names = value;
            }
        }

        protected virtual String GetNameToCheck(MemberInfo memberInfo)
        {
            return IgnoreCase ? memberInfo.Name.ToLowerInvariant() : memberInfo.Name;
        }

        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            // prep it, so that we can just use the IEnumerable
            var namesList = new List<string>();
                namesList.AddRange(IgnoreCase
                        ? from o in Names select o.ToLowerInvariant()
                        : Names);

            if (NameHandling == NameHandlingType.Whole)
            {
                return memberInfos.Where(memberInfo => namesList.Contains(GetNameToCheck(memberInfo))).ToArray();
            }
            else if (NameHandling == NameHandlingType.StartsWith)
            {
                return memberInfos.Where(memberInfo => namesList.Any(name => GetNameToCheck(memberInfo).StartsWith(name))).ToArray();
            }
            else if (NameHandling == NameHandlingType.EndsWith)
            {
                return memberInfos.Where(memberInfo => namesList.Any(name => GetNameToCheck(memberInfo).EndsWith(name))).ToArray();
            }
            else // if (NameEvaluationHandling == NameEvaluationHandlingType.Contains)
            {
                return memberInfos.Where(memberInfo => namesList.Any(name => GetNameToCheck(memberInfo).Contains(name))).ToArray();
            }
        }
    }
}
