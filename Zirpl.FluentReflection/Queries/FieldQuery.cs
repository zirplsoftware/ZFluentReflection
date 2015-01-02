﻿using System;
using System.Reflection;

namespace Zirpl.FluentReflection
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

        ITypeSubQuery<FieldInfo, IFieldQuery> IFieldQuery.OfFieldType()
        {
            return new TypeSubQuery<FieldInfo, IFieldQuery>(this, _fieldTypeCriteria);
        }
    }
}
