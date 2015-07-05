using System;
using System.Reflection;

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

        IMethodQuery IMethodQuery.WithParameters(Type[] typesOfParameters)
        {
            _methodCriteria.ParameterTypes = typesOfParameters;
            return this;
        }

        IMethodQuery IMethodQuery.OfReturnType(Action<ITypeCriteriaBuilder> builder)
        {
            builder(new TypeCriteriaBuilder(_methodCriteria.ReturnTypeCriteria));
            return this;
        }
    }
}
