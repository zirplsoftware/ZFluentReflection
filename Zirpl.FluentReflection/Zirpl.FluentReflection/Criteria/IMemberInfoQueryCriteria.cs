using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal interface IMemberInfoQueryCriteria
    {
        MemberInfo[] FilterMatches(MemberInfo[] memberInfos);
    }
}