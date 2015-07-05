using System;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class NestedTypeQuery : NamedMemberQueryBase<Type, INestedTypeQuery>, 
        INestedTypeQuery
    {
        private readonly TypeCriteria _typeCriteria;
        internal NestedTypeQuery(Type type)
            :base(type)
        {
            MemberTypeFlagsBuilder.NestedType = true;
            _typeCriteria = new TypeCriteria(TypeSource.Self);
            QueryCriteriaList.Add(_typeCriteria);
        }

        INestedTypeQuery INestedTypeQuery.OfType(Action<ITypeCriteriaBuilder> builder)
        {
            builder(new TypeCriteriaBuilder(_typeCriteria));
            return this;
        }
    }
}
