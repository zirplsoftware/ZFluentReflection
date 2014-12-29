using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class EventHandlerTypeEvaluator :TypeEvaluator
    {
        public override bool IsMatch(MemberInfo memberInfo)
        {
            return base.IsMatch(((EventInfo)memberInfo).EventHandlerType);
        }
    }
}
