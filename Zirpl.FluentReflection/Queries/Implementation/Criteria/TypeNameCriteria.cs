using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries.Criteria
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

        protected internal override bool ShouldRun
        {
            get { return Names != null; }
        }
    }
}