using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class FieldQuery : NamedTypeMemberQueryBase<FieldInfo, IFieldQuery>, 
        IFieldQuery
    {
        private readonly FieldTypeCriteria _fieldTypeCriteria;

        internal FieldQuery(Type type)
            :base(type)
        {
            _memberTypeCriteria.Field = true;
            _fieldTypeCriteria = new FieldTypeCriteria();
            _matchEvaluators.Add(_fieldTypeCriteria);
        }

        ITypeQuery<FieldInfo, IFieldQuery> IFieldQuery.OfFieldType()
        {
            return new TypeSubQuery<FieldInfo, IFieldQuery>(this, _fieldTypeCriteria);
        }
    }
}
