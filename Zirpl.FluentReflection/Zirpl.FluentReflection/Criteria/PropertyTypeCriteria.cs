using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyTypeCriteria :TypeCriteria
    {
        protected override Type GetTypeToCheck(MemberInfo memberInfo)
        {
            return ((PropertyInfo)memberInfo).PropertyType;
        }
    }
}
