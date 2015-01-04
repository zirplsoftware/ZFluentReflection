using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IPropertyQuery : INamedMemberQuery<PropertyInfo, IPropertyQuery>
    {
        ITypeSubQuery<PropertyInfo, IPropertyQuery> OfPropertyType();
        IPropertyQuery WithGetter();
        IPropertyQuery WithSetter();
        IPropertyQuery WithGetterAndSetter();
    }
}