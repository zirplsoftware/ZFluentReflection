using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class TypeNameCriteria : NameCriteria
    {
        internal bool UseFullName { get; set; }

        protected override string GetNameToCheck(MemberInfo memberInfo)
        {
            var type = (Type) memberInfo;
            var name = UseFullName ? type.FullName : type.Name;
            name = IgnoreCase ? name.ToLowerInvariant() : name;
            return name;
        }

        protected internal override bool ShouldRunFilter
        {
            get { return Names != null; }
        }
    }
}