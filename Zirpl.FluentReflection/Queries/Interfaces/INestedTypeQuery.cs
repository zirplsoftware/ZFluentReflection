using System;

namespace Zirpl.FluentReflection
{
    public interface INestedTypeQuery : INamedMemberQuery<Type, INestedTypeQuery>
    {
        ITypeSubQuery<Type, INestedTypeQuery> OfType();
    }
}