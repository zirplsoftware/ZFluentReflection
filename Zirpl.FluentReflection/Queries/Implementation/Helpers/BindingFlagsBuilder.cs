using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class BindingFlagsBuilder
    {
        private readonly ScopeCriteria _memberScopeCriteria;
        private readonly AccessibilityCriteria _memberAccessibilityCriteria;
        private readonly MemberNameCriteria _memberNameEvalulator;

        internal BindingFlagsBuilder(AccessibilityCriteria memberAccessibilityCriteria, ScopeCriteria memberScopeCriteria, MemberNameCriteria memberNameEvalulator)
        {
            _memberScopeCriteria = memberScopeCriteria;
            _memberAccessibilityCriteria = memberAccessibilityCriteria;
            _memberNameEvalulator = memberNameEvalulator;
        }

        internal BindingFlags BindingFlags
        {
            get
            {
                var bindings = default(BindingFlags);
                bindings = _memberAccessibilityCriteria.Public ? bindings | BindingFlags.Public : bindings;
                bindings = _memberAccessibilityCriteria.Private || _memberAccessibilityCriteria.Family || _memberAccessibilityCriteria.FamilyOrAssembly || _memberAccessibilityCriteria.Assembly
                            ? bindings | BindingFlags.NonPublic : bindings;
                bindings = _memberScopeCriteria.Instance ? bindings | BindingFlags.Instance : bindings;
                bindings = _memberScopeCriteria.Static ? bindings | BindingFlags.Static : bindings;
                bindings = _memberScopeCriteria.Static && _memberScopeCriteria.DeclaredOnBaseTypes ? bindings | BindingFlags.FlattenHierarchy : bindings;
                bindings = _memberNameEvalulator.IgnoreCase ? bindings | BindingFlags.IgnoreCase : bindings;
                bindings = _memberScopeCriteria.DeclaredOnThisType && !_memberScopeCriteria.DeclaredOnBaseTypes ? bindings | BindingFlags.DeclaredOnly : bindings;
                if (!bindings.HasFlag(BindingFlags.Instance)
                    && !bindings.HasFlag(BindingFlags.Static))
                {
                    bindings = bindings | BindingFlags.Instance;
                }
                if (!bindings.HasFlag(BindingFlags.Public)
                    && !bindings.HasFlag(BindingFlags.NonPublic))
                {
                    bindings = bindings | BindingFlags.Public;
                }
                return bindings;
            }
        }
    }
}
