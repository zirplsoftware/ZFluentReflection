using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class NameCriteria : MemberInfoQueryCriteriaBase
    {
        private String _name;
        private IEnumerable<String> _names;

        internal bool StartsWith { get; set; }
        internal bool EndsWith { get; set; }
        internal bool Contains { get; set; }
        internal bool Any { get; set; }
        internal bool IgnoreCase { get; set; }
        internal String Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException("value");
                if (_name != null) throw new InvalidOperationException("Cannot call Name twice");
                if (_names != null) throw new InvalidOperationException("Cannot call both Name and Names.");

                _name = value;
            }
        }
        internal IEnumerable<String> Names
        {
            get { return _names; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (!value.Any()) throw new ArgumentException("names must have at least one entry", "value");
                if (value.Any(String.IsNullOrEmpty)) throw new ArgumentException("An entry in the names provided was null", "value");
                if (_names != null) throw new InvalidOperationException("Cannot call Names twice.");
                if (_name != null) throw new InvalidOperationException("Cannot call both Name and Names.");

                _names = value;
            }
        }

        protected virtual String GetNameToCheck(MemberInfo memberInfo)
        {
            return IgnoreCase ? memberInfo.Name.ToLowerInvariant() : memberInfo.Name;
        }

        protected override MemberInfo[] DoFilterMatches(MemberInfo[] memberInfos)
        {
            // prep it, so that we can just use the IEnumerable
            var namesList = new List<string>();
            if (Name != null)
            {
                namesList.Add(IgnoreCase ? Name.ToLowerInvariant() : Name);
            }
            else // we know that names is present
            {
                namesList.AddRange(IgnoreCase
                        ? from o in Names select o.ToLowerInvariant()
                        : Names);
            }

            if (StartsWith)
            {
                return memberInfos.Where(o => namesList.Any(name => name.StartsWith(GetNameToCheck(o)))).ToArray();
            }
            else if (EndsWith)
            {
                return memberInfos.Where(o => namesList.Any(name => name.EndsWith(GetNameToCheck(o)))).ToArray();
            }
            else if (Contains)
            {
                return memberInfos.Where(o => namesList.Any(name => name.Contains(GetNameToCheck(o)))).ToArray();
            }
            else
            {
                return memberInfos.Where(o => namesList.Contains(GetNameToCheck(o))).ToArray();
            }
        }

        protected override bool ShouldRunFilter
        {
            get
            {
                // unfortunately we CANNOT assume that names were handled by the service
                // because sometimes this class will be used for type names, not member names
                // so any criteria requires a check
                return Name != null || Names != null;
            }
        }
    }
}
