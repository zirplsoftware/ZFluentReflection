using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMemberQuery : INamedMemberQuery<MemberInfo, IMemberQuery>
    {
        IMemberTypeSubQuery OfMemberType();
    }
}