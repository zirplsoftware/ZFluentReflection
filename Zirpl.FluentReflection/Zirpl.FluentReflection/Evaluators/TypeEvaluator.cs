﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class TypeEvaluator : IMatchEvaluator
    {
        // TODO: how can these be used? Type.FindInterfaces, Type.IsIstanceOf, Type.IsSubClassOf

        internal NameEvaluator NameEvaluator { get; private set; }
        internal TypeFullNameEvaluator FullNameEvaluator { get; private set; }
        internal Type AssignableFrom { get; set; }
        internal IEnumerable<Type> AssignableFroms { get; set; }
        internal Type AssignableTo { get; set; }
        internal IEnumerable<Type> AssignableTos { get; set; }
        internal bool Any { get; set; }

        internal TypeEvaluator()
        {
            NameEvaluator = new NameEvaluator();
            FullNameEvaluator = new TypeFullNameEvaluator();
        }

        public bool IsMatchCheckRequired()
        {
            _checkLocal = AssignableFrom != null
                          || AssignableFroms != null
                          || AssignableTo != null
                          || AssignableTos != null;
            _checkNameEvaluator = NameEvaluator.IsMatchCheckRequired();
            _checkFullNameEvaluator = FullNameEvaluator.IsMatchCheckRequired();
            return _checkLocal
                   || _checkNameEvaluator
                   || _checkFullNameEvaluator;
        }

        private bool _checkNameEvaluator;
        private bool _checkFullNameEvaluator;
        private bool _checkLocal;

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

        public virtual bool IsMatch(MemberInfo memberInfo)
        {
            var type = (Type) memberInfo;
            if (_checkLocal)
            {
                if (type == null) return false;
                if (AssignableFrom != null && !type.IsAssignableFrom(AssignableFrom)) return false;
                if (AssignableTo != null && !AssignableTo.IsAssignableFrom(type)) return false;
                if (AssignableFroms != null && !Any && !AssignableFroms.All(type.IsAssignableFrom)) return false;
                if (AssignableTos != null && !Any && !AssignableTos.All(o => o.IsAssignableFrom(type))) return false;
                if (AssignableFroms != null && Any && !AssignableFroms.Any(type.IsAssignableFrom)) return false;
                if (AssignableTos != null && Any && !AssignableTos.All(o => o.IsAssignableFrom(type))) return false;
            }
            if (_checkFullNameEvaluator)
            {
                if (!FullNameEvaluator.IsMatch(type)) return false;
            }
            if (_checkNameEvaluator)
            {
                if (!NameEvaluator.IsMatch(type)) return false;
            }
            return true;
        }

    }
}
