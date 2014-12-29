using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IFieldQuery : INamedMemberQuery<FieldInfo, IFieldQuery>
    {
        ITypeQuery<FieldInfo, IFieldQuery> OfFieldType();
    }
}