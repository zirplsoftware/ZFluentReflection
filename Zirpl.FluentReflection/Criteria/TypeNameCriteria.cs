using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class TypeNameCriteria : NameCriteria
    {
        protected override string GetNameToCheck(MemberInfo memberInfo)
        {
            var type = (Type) memberInfo;
            return IgnoreCase ? type.Name.ToLowerInvariant() : type.FullName;
        }

        protected internal override bool ShouldRunFilter
        {
            get { return Name != null || Names != null; }
        }
    }
}