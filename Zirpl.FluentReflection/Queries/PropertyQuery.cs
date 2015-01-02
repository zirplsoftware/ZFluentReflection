﻿using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyQuery : NamedMemberQueryBase<PropertyInfo, IPropertyQuery>,
        IPropertyQuery
    {
        private readonly PropertyReadWriteCriteria _readWriteCriteria;
        private readonly TypeCriteria _propertyTypeCriteria;

        internal PropertyQuery(Type type)
            :base(type)
        {
            _readWriteCriteria = new PropertyReadWriteCriteria();
            _propertyTypeCriteria = new TypeCriteria(TypeSource.PropertyType);
            MemberTypeFlagsBuilder.Property = true;
            QueryCriteriaList.Add(_readWriteCriteria);
            QueryCriteriaList.Add(_propertyTypeCriteria);
        }

        ITypeSubQuery<PropertyInfo, IPropertyQuery> IPropertyQuery.OfPropertyType()
        {
            return new TypeSubQuery<PropertyInfo, IPropertyQuery>(this, _propertyTypeCriteria);
        }

        IPropertyQuery IPropertyQuery.WithGetter()
        {
            _readWriteCriteria.CanRead = true;
            return this;
        }

        IPropertyQuery IPropertyQuery.WithSetter()
        {
            _readWriteCriteria.CanWrite = true;
            return this;
        }

        IPropertyQuery IPropertyQuery.WithGetterAndSetter()
        {
            _readWriteCriteria.CanRead = true;
            _readWriteCriteria.CanWrite = true;
            return this;
        }
    }
}
