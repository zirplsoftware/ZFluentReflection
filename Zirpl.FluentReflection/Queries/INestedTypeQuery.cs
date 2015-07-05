using System;

namespace Zirpl.FluentReflection.Queries
{
    public interface INestedTypeQuery : INamedMemberQuery<Type, INestedTypeQuery>
    {
        INestedTypeQuery OfType(Action<ITypeCriteriaBuilder> builder);
    }
}