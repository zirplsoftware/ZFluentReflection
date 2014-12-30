using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class FieldTypeCriteria :TypeCriteria
    {
        protected override Type GetTypeToCheck(MemberInfo memberInfo)
        {
            return ((FieldInfo)memberInfo).FieldType;
        }
    }
}
