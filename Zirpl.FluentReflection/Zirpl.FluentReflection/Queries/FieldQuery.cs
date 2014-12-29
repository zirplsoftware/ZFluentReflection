﻿using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class FieldQuery : NamedTypeMemberQueryBase<FieldInfo, IFieldQuery>, 
        IFieldQuery
    {
        private readonly FieldTypeEvaluator _fieldTypeEvaluator;

        internal FieldQuery(Type type)
            :base(type)
        {
            _memberTypeEvaluator.Field = true;
            _fieldTypeEvaluator = new FieldTypeEvaluator();
            _matchEvaluators.Add(_fieldTypeEvaluator);
        }

        ITypeQuery<FieldInfo, IFieldQuery> IFieldQuery.OfFieldType()
        {
            return new TypeSubQuery<FieldInfo, IFieldQuery>(this, _fieldTypeEvaluator);
        }
    }
}
