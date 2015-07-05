using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IFieldQuery : INamedMemberQuery<FieldInfo, IFieldQuery>
    {
        IFieldQuery OfFieldType(Action<ITypeCriteriaBuilder> builder);
    }
}