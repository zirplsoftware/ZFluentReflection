using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class TypeCriteria : MemberInfoQueryCriteriaBase
    {
        // TODO: how can these be used? Type.FindInterfaces, Type.IsIstanceOf, Type.IsSubClassOf

        internal TypeNameCriteria NameCriteria { get; private set; }
        internal TypeFullNameCriteria FullNameCriteria { get; private set; }
        internal Type AssignableFrom { get; set; }
        internal IEnumerable<Type> AssignableFroms { get; set; }
        internal Type AssignableTo { get; set; }
        internal IEnumerable<Type> AssignableTos { get; set; }
        internal bool Any { get; set; }

        internal TypeCriteria()
        {
            NameCriteria = new TypeNameCriteria();
            FullNameCriteria = new TypeFullNameCriteria();
            SubFilters.Add(NameCriteria);
            SubFilters.Add(FullNameCriteria);
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


        protected override MemberInfo[] DoFilterMatches(MemberInfo[] memberInfos)
        {
            return memberInfos.Where(o => IsMatch(GetTypeToCheck(o))).ToArray();
        }

        protected virtual Type GetTypeToCheck(MemberInfo memberInfo)
        {
            return (Type) memberInfo;
        }

        protected virtual bool IsMatch(Type type)
        {
            if (type == null) return false;
            if (AssignableFrom != null && !type.IsAssignableFrom(AssignableFrom)) return false;
            if (AssignableTo != null && !AssignableTo.IsAssignableFrom(type)) return false;
            if (AssignableFroms != null && !Any && !AssignableFroms.All(type.IsAssignableFrom)) return false;
            if (AssignableTos != null && !Any && !AssignableTos.All(o => o.IsAssignableFrom(type))) return false;
            if (AssignableFroms != null && Any && !AssignableFroms.Any(type.IsAssignableFrom)) return false;
            if (AssignableTos != null && Any && !AssignableTos.All(o => o.IsAssignableFrom(type))) return false;
            return true;
        }

        protected internal override bool ShouldRunFilter
        {
            get
            {
                return AssignableFrom != null
                       || AssignableFroms != null
                       || AssignableTo != null
                       || AssignableTos != null;
            }
        }
    }
}
