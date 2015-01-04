using System.Reflection;

namespace Zirpl.FluentReflection.Queries.Criteria
{
    internal interface IMemberInfoQueryCriteria
    {
        MemberInfo[] GetMatches(MemberInfo[] memberInfos);
    }
}