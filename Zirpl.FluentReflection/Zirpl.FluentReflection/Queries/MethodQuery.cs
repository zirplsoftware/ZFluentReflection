using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MethodQuery : NamedMemberQueryBase<MethodInfo, IMethodQuery>,
        IMethodQuery
    {
        private readonly MethodReturnTypeCriteria _returnTypeCriteria;

        internal MethodQuery(Type type)
            :base(type)
        {
            _returnTypeCriteria = new MethodReturnTypeCriteria();
            _memberTypeCriteria.Method = true;
            _matchEvaluators.Add(_returnTypeCriteria);
        }

        ITypeSubQuery<MethodInfo, IMethodQuery> IMethodQuery.OfReturnType()
        {
            return new TypeSubQuery<MethodInfo, IMethodQuery>(this, _returnTypeCriteria);
        }
    }
}
