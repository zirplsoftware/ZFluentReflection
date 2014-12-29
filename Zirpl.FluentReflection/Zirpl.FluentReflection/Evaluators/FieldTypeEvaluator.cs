using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class FieldTypeEvaluator :TypeEvaluator
    {
        public override bool IsMatch(MemberInfo memberInfo)
        {
            return base.IsMatch(((FieldInfo)memberInfo).FieldType);
        }
    }
}
