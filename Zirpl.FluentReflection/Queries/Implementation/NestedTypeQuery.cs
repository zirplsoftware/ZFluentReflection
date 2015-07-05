using System;
using Zirpl.FluentReflection.Queries.Implementation.Criteria;
using Zirpl.FluentReflection.Queries.Implementation.CriteriaBuilders;

namespace Zirpl.FluentReflection.Queries.Implementation
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
