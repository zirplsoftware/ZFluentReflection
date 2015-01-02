using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface INamedMemberQuery<out TMemberInfo, out TMemberQuery> : IMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo
        where TMemberQuery : INamedMemberQuery<TMemberInfo, TMemberQuery>
    {
        INameSubQuery<TMemberInfo, INamedMemberQuery<TMemberInfo, TMemberQuery>> Named();
        TMemberQuery Named(String name);
    }
}