using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class FieldQuery : NamedMemberQueryBase<FieldInfo, IFieldQuery>, 
        IFieldQuery
    {
        private readonly FieldTypeCriteria _fieldTypeCriteria;

        internal FieldQuery(Type type)
            :base(type)
        {
            _memberTypeCriteria.Field = true;
            _fieldTypeCriteria = new FieldTypeCriteria();
            _queryCriteriaList.Add(_fieldTypeCriteria);
        }

        ITypeSubQuery<FieldInfo, IFieldQuery> IFieldQuery.OfFieldType()
        {
            return new TypeSubQuery<FieldInfo, IFieldQuery>(this, _fieldTypeCriteria);
        }
    }
}
