using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IFieldQuery : INamedMemberQuery<FieldInfo, IFieldQuery>
    {
        ITypeSubQuery<FieldInfo, IFieldQuery> OfFieldType();
    }
}