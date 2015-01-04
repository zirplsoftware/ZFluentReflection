using System;

namespace Zirpl.FluentReflection.Queries
{
    public interface INestedTypeQuery : INamedMemberQuery<Type, INestedTypeQuery>
    {
        ITypeSubQuery<Type, INestedTypeQuery> OfType();
    }
}