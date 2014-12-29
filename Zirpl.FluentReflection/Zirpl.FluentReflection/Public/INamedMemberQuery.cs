using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface INamedMemberQuery<out TMemberInfo, out TMemberQuery> : IMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo
        where TMemberQuery : INamedMemberQuery<TMemberInfo, TMemberQuery>
    {
        INameQuery<TMemberInfo, INamedMemberQuery<TMemberInfo, TMemberQuery>> Named();
    }
}