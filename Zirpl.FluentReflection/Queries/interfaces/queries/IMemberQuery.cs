using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMemberQuery : INamedMemberQuery<MemberInfo, IMemberQuery>
    {
        IMemberQuery OfMemberType(Action<IMemberTypeCriteriaBuilder> builder);
    }
}