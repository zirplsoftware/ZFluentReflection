using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal interface IMemberInfoQueryCriteria
    {
        MemberInfo[] GetMatches(MemberInfo[] memberInfos);
    }
}