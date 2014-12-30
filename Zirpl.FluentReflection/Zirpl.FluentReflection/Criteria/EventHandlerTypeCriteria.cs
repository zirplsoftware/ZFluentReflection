using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class EventHandlerTypeCriteria :TypeCriteria
    {
        public override bool IsMatch(MemberInfo memberInfo)
        {
            return base.IsMatch(((EventInfo)memberInfo).EventHandlerType);
        }
    }
}
