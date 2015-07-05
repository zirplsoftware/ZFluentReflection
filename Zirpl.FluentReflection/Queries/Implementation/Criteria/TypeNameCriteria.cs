using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class TypeNameCriteria : NameCriteria
    {
        private readonly TypeSource _typeSource;
        internal bool UseFullName { get; set; }

        internal TypeNameCriteria(TypeSource typeSource)
        {
            _typeSource = typeSource;
        }

        protected override string GetNameToCheck(MemberInfo memberInfo)
        {
            var type = memberInfo.GetAssociatedType(_typeSource);
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