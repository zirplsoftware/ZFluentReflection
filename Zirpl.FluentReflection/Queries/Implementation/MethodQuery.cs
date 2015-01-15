﻿using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.Criteria;
using Zirpl.FluentReflection.Queries.Implementation.SubQueries;

namespace Zirpl.FluentReflection.Queries.Implementation
{
    internal sealed class MethodQuery : NamedMemberQueryBase<MethodInfo, IMethodQuery>,
        IMethodQuery
    {
        private readonly MethodCriteria _methodCriteria;

        internal MethodQuery(Type type)
            :base(type)
        {
            _methodCriteria = new MethodCriteria();
            MemberTypeFlagsBuilder.Method = true;
            QueryCriteriaList.Add(_methodCriteria);
        }

        ITypeSubQuery<MethodInfo, IMethodQuery> IMethodQuery.OfReturnType()
        {
            return new TypeSubQuery<MethodInfo, IMethodQuery>(this, _methodCriteria.ReturnTypeCriteria);
        }

        IMethodQuery IMethodQuery.WithParameters(Type[] typesOfParameters)
        {
            _methodCriteria.ParameterTypes = typesOfParameters;
            return this;
        }
    }
}
