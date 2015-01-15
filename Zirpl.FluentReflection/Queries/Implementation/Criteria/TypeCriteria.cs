using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries.Implementation.Criteria
{
    internal class TypeCriteria : MemberInfoQueryCriteriaBase
    {
        private IEnumerable<Type> _assignableFroms;
        private IEnumerable<Type> _assignableTos;
        internal TypeSource TypeSource { get; private set; }
        internal TypeNameCriteria NameCriteria { get; private set; }
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

        internal TypeCriteria(TypeSource typeSource)
        {
            TypeSource = typeSource;
            NameCriteria = new TypeNameCriteria();
            SubCriterias.Add(NameCriteria);
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
            return memberInfos.Where(o => IsMatch(GetTypeToCheck(o))).ToArray();
        }

        // only internal to be able to test it
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
