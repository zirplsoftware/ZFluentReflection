﻿using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MethodQuery : NamedTypeMemberQueryBase<MethodInfo, IMethodQuery>,
        IMethodQuery
    {
        private readonly MethodReturnTypeEvaluator _returnTypeEvaluator;

        internal MethodQuery(Type type)
            :base(type)
        {
            _returnTypeEvaluator = new MethodReturnTypeEvaluator();
            _memberTypeEvaluator.Method = true;
            _matchEvaluators.Add(_returnTypeEvaluator);
        }

        ITypeQuery<MethodInfo, IMethodQuery> IMethodQuery.OfReturnType()
        {
            return new TypeSubQuery<MethodInfo, IMethodQuery>(this, _returnTypeEvaluator);
        }
    }
}
