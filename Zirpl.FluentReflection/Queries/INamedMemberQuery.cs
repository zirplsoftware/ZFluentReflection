using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface INamedMemberQuery<out TMemberInfo, out TMemberQuery> : IMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo
        where TMemberQuery : INamedMemberQuery<TMemberInfo, TMemberQuery>
    {
        TMemberQuery Named(Action<INameCriteriaBuilder> builder);
    }
}