using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.Criteria;
using Zirpl.FluentReflection.Queries.Implementation.CriteriaBuilders;

namespace Zirpl.FluentReflection.Queries.Implementation
{
    internal sealed class PropertyQuery : NamedMemberQueryBase<PropertyInfo, IPropertyQuery>,
        IPropertyQuery
    {
        private readonly PropertyCriteria _propertyCriteria;

        internal PropertyQuery(Type type)
            :base(type)
        {
            _propertyCriteria = new PropertyCriteria();
            MemberTypeFlagsBuilder.Property = true;
            QueryCriteriaList.Add(_propertyCriteria);
        }

        IPropertyQuery IPropertyQuery.OfPropertyType(Action<ITypeCriteriaBuilder> builder)
        {
            builder(new TypeCriteriaBuilder(_propertyCriteria.PropertyTypeCriteria));
            return this;
        }

        IPropertyQuery IPropertyQuery.WithGetter()
        {
            _propertyCriteria.CanRead = true;
            return this;
        }

        IPropertyQuery IPropertyQuery.WithSetter()
        {
            _propertyCriteria.CanWrite = true;
            return this;
        }

        IPropertyQuery IPropertyQuery.WithGetterAndSetter()
        {
            _propertyCriteria.CanRead = true;
            _propertyCriteria.CanWrite = true;
            return this;
        }
    }
}
