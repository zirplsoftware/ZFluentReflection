using System;
using Zirpl.FluentReflection.Queries.Criteria;
using Zirpl.FluentReflection.Queries.SubQueries;

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

        ITypeSubQuery<Type, INestedTypeQuery> INestedTypeQuery.OfType()
        {
            return new TypeSubQuery<Type, INestedTypeQuery>(this, _typeCriteria);
        }
    }
}
