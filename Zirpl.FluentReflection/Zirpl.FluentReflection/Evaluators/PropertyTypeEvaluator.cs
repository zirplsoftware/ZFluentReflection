using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyTypeEvaluator :TypeEvaluator
    {
        public override bool IsMatch(MemberInfo memberInfo)
        {
            return base.IsMatch(((PropertyInfo)memberInfo).PropertyType);
        }
    }
}
