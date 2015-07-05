using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class TypeCompatibilityCriteria : MemberInfoCriteriaBase
    {
        private readonly TypeSource _typeSource;
        private IEnumerable<Type> _assignableFroms;
        private IEnumerable<Type> _assignableTos;
        internal bool AssignableFromAny { get; set; }
        internal IEnumerable<Type> AssignableFroms
        {
            get { return _assignableFroms; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (!value.Any()) throw new ArgumentException("Must have at least one entry", "value");
                if (value.Any(type => type == null)) throw new ArgumentException("An entry is null", "value");

                _assignableFroms = value;
            }
        }
        internal bool AssignableToAny { get; set; }
        internal IEnumerable<Type> AssignableTos
        {
            get { return _assignableTos; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (!value.Any()) throw new ArgumentException("Must have at least one entry", "value");
                if (value.Any(type => type == null)) throw new ArgumentException("An entry is null", "value");

                _assignableTos = value;
            }
        }

        internal TypeCompatibilityCriteria(TypeSource typeSource)
        {
            _typeSource = typeSource;
        }

        // TODO: implement all these
        //private bool _isValueType;
        //private bool _isNullableValueType;
        //private bool _isValueTypeOrNullableValueType;
        //private bool _isEnum;
        //private bool _isNullableEnum;
        //private bool _isEnumOrIsNullableEnum;
        //private bool _isClass;
        //private bool _isInterface;
        //private bool _isClassOrInterface;
        //private bool _isPrimtive;
        //private Type _implementingInterface;
        //private IEnumerable<Type> _implementingAllInterfaces;
        //private IEnumerable<Type> _implementingAnyInterfaces;
        //private Type _exact;
        //private IEnumerable<Type> _exactAny;


        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            return memberInfos.Where(o => IsMatch(o.GetAssociatedType(_typeSource))).ToArray();
        }

        private bool IsMatch(Type type)
        {
            // TODO: how can these be used? Type.FindInterfaces, Type.IsIstanceOf, Type.IsSubClassOf

            if (type == null) return false;
            if (AssignableFroms != null && AssignableFromAny && !AssignableFroms.Any(type.IsAssignableFrom)) return false;
            if (AssignableFroms != null && !AssignableFromAny && !AssignableFroms.All(type.IsAssignableFrom)) return false;
            if (AssignableTos != null && AssignableToAny && !AssignableTos.All(o => o.IsAssignableFrom(type))) return false;
            if (AssignableTos != null && !AssignableToAny && !AssignableTos.All(o => o.IsAssignableFrom(type))) return false;
            return true;
        }

        protected internal override bool ShouldRun
        {
            get
            {
                return AssignableFroms != null
                       || AssignableTos != null;
            }
        }
    }
}
