using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyQuery : NamedMemberQueryBase<PropertyInfo, IPropertyQuery>,
        IPropertyQuery
    {
        private readonly PropertyReadWriteCriteria _readWriteCriteria;
        private readonly PropertyTypeCriteria _propertyTypeCriteria;

        internal PropertyQuery(Type type)
            :base(type)
        {
            _readWriteCriteria = new PropertyReadWriteCriteria();
            _propertyTypeCriteria = new PropertyTypeCriteria();
            _memberTypeCriteria.Property = true;
            _queryCriteriaList.Add(_readWriteCriteria);
            _queryCriteriaList.Add(_propertyTypeCriteria);
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
