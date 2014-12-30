using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyTypeCriteria :TypeCriteria
    {
        public override bool IsMatch(MemberInfo memberInfo)
        {
            return base.IsMatch(((PropertyInfo)memberInfo).PropertyType);
        }
    }
}
