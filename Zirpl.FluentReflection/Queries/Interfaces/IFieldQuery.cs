using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IFieldQuery : INamedMemberQuery<FieldInfo, IFieldQuery>
    {
        ITypeSubQuery<FieldInfo, IFieldQuery> OfFieldType();
    }
}