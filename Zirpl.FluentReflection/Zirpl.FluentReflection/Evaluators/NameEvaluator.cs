using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class NameEvaluator : IMatchEvaluator
    {
        private String _name;
        private IEnumerable<String> _names;

        internal bool StartsWith { get; set; }
        internal bool EndsWith { get; set; }
        internal bool Contains { get; set; }
        internal bool All { get; set; }
        internal bool Any { get; set; }


        public virtual bool IsMatchCheckRequired()
        {
            // unfortunately we CANNOT assume that names were handled by the service
            // because sometimes this class will be used for type names, not member names
            // so any criteria requires a check
            //
            var requiresCheck = _name != null || _names != null;
            if (requiresCheck)
            {
                if (_name != null)
                {
                    // prep it, so that we can just use the IEnumerable
                    _names = new String[] {IgnoreCase ? _name.ToLowerInvariant() : _name};
                    _name = null;
                }
                else // we know that names is present
                {
                    _names = IgnoreCase
                            ? from o in _names select o.ToLowerInvariant()
                            : _names;
                }
            }
            
            return requiresCheck;
        }

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
        internal bool IgnoreCase { get; set; }

        public bool IsMatch(MemberInfo memberInfo)
        {
            // we CAN assume that IF we got here, the _names property has been FULLY set up for use here, so just go
            var nameToCheck = GetNameToCheck(memberInfo);
            return _names.Contains(IgnoreCase
                    ? nameToCheck.ToLowerInvariant()
                    : nameToCheck);
        }

        protected virtual String GetNameToCheck(MemberInfo memberInfo)
        {
            return memberInfo.Name;
        }
    }
}
