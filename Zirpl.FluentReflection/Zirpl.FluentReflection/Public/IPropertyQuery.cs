using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IPropertyQuery : INamedMemberQuery<PropertyInfo, IPropertyQuery>
    {
        ITypeQuery<PropertyInfo, IPropertyQuery> OfPropertyType();
        IPropertyQuery WithGetter();
        IPropertyQuery WithSetter();
        IPropertyQuery WithGetterAndSetter();
    }
}