using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class TypeCriteria : MemberInfoQueryCriteriaBase
    {
        // TODO: how can these be used? Type.FindInterfaces, Type.IsIstanceOf, Type.IsSubClassOf

        internal TypeSource TypeSource { get; private set; }
        internal TypeNameCriteria NameCriteria { get; private set; }
        internal IEnumerable<Type> AssignableFroms { get; set; }
        internal IEnumerable<Type> AssignableTos { get; set; }
        internal bool Any { get; set; }

        internal TypeCriteria(TypeSource typeSource)
        {
            TypeSource = typeSource;
            NameCriteria = new TypeNameCriteria();
            SubFilters.Add(NameCriteria);
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

        // only internal to be able to check it
        protected internal Type GetTypeToCheck(MemberInfo memberInfo)
        {
            switch (TypeSource)
            {
                case TypeSource.Self:
                    return (Type)memberInfo;
                case TypeSource.EventHandlerType:
                    return ((EventInfo)memberInfo).EventHandlerType;
                case TypeSource.FieldType:
                    return ((FieldInfo)memberInfo).FieldType;
                case TypeSource.MethodReturnType:
                    return ((MethodInfo)memberInfo).ReturnType;
                case TypeSource.PropertyType:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new Exception("Unknown TypeSource: " + TypeSource.ToString());
            }
        }

        protected virtual bool IsMatch(Type type)
        {
            if (type == null) return false;
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
                return AssignableFroms != null
                       || AssignableTos != null;
            }
        }
    }
}
