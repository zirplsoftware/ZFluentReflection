using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal interface IMemberInfoCriteria
    {
        MemberInfo[] GetMatches(MemberInfo[] memberInfos);
    }
}