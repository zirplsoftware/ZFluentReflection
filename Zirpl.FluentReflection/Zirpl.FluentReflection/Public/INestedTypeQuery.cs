using System;

namespace Zirpl.FluentReflection
{
    public interface INestedTypeQuery : INamedMemberQuery<Type, INestedTypeQuery>
    {
        ITypeQuery<Type, INestedTypeQuery> OfType();
    }
}