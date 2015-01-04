using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Criteria;
using Zirpl.FluentReflection.Queries.SubQueries;

namespace Zirpl.FluentReflection.Queries
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
    }
}
