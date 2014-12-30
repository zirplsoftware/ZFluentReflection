using System;

namespace Zirpl.FluentReflection
{
    internal sealed class NestedTypeQuery : NamedMemberQueryBase<Type, INestedTypeQuery>, 
        INestedTypeQuery
    {
        private readonly TypeCriteria _typeCriteria;
        internal NestedTypeQuery(Type type)
            :base(type)
        {
            _memberTypeCriteria.NestedType = true;
            _typeCriteria = new TypeCriteria();
            _matchEvaluators.Add(_typeCriteria);
        }

        ITypeSubQuery<Type, INestedTypeQuery> INestedTypeQuery.OfType()
        {
            return new TypeSubQuery<Type, INestedTypeQuery>(this, _typeCriteria);
        }
    }
}
