using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IPropertyQuery : INamedMemberQuery<PropertyInfo, IPropertyQuery>
    {
        IPropertyQuery OfPropertyType(Action<ITypeCriteriaBuilder> build);
        IPropertyQuery WithGetter();
        IPropertyQuery WithSetter();
        IPropertyQuery WithGetterAndSetter();
    }
}