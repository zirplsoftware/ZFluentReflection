using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class EventHandlerTypeCriteria :TypeCriteria
    {
        protected override Type GetTypeToCheck(MemberInfo memberInfo)
        {
            return ((EventInfo)memberInfo).EventHandlerType;
        }
    }
}
