using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class FieldQuery : NamedMemberQueryBase<FieldInfo, IFieldQuery>, 
        IFieldQuery
    {
        private readonly TypeCriteria _fieldTypeCriteria;

        internal FieldQuery(Type type)
            :base(type)
        {
            MemberTypeFlagsBuilder.Field = true;
            _fieldTypeCriteria = new TypeCriteria(TypeSource.FieldType);
            QueryCriteriaList.Add(_fieldTypeCriteria);
        }

        IFieldQuery IFieldQuery.OfFieldType(Action<ITypeCriteriaBuilder> builder)
        {
            builder(new TypeCriteriaBuilder(_fieldTypeCriteria));
            return this;
        }
    }
}
