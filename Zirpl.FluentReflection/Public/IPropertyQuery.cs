using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IPropertyQuery : INamedMemberQuery<PropertyInfo, IPropertyQuery>
    {
        ITypeSubQuery<PropertyInfo, IPropertyQuery> OfPropertyType();
        IPropertyQuery WithGetter();
        IPropertyQuery WithSetter();
        IPropertyQuery WithGetterAndSetter();
    }
}