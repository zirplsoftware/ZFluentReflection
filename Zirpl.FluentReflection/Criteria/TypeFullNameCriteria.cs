using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class TypeFullNameCriteria : NameCriteria
    {
        protected override string GetNameToCheck(MemberInfo memberInfo)
        {
            var type = (Type) memberInfo;
            return IgnoreCase ? type.FullName.ToLowerInvariant() : type.FullName;
        }
    }
}