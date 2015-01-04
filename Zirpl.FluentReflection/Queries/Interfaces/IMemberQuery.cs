using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMemberQuery : INamedMemberQuery<MemberInfo, IMemberQuery>
    {
        IMemberTypeSubQuery OfMemberType();
    }
}