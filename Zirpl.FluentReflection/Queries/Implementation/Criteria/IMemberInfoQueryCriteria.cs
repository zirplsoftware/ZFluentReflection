using System.Reflection;

namespace Zirpl.FluentReflection.Queries.Implementation.Criteria
{
    internal interface IMemberInfoQueryCriteria
    {
        MemberInfo[] GetMatches(MemberInfo[] memberInfos);
    }
}