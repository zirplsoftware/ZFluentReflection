using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class BindingFlagsBuilder
    {
        private readonly MemberScopeCriteria _memberScopeCriteria;
        private readonly MemberAccessibilityCriteria _memberAccessibilityCriteria;
        private readonly MemberNameCriteria _memberNameEvalulator;

        internal BindingFlagsBuilder(MemberAccessibilityCriteria memberAccessibilityCriteria, MemberScopeCriteria memberScopeCriteria, MemberNameCriteria memberNameEvalulator)
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
                bindings = _memberAccessibilityCriteria.Private || _memberAccessibilityCriteria.Protected || _memberAccessibilityCriteria.ProtectedInternal || _memberAccessibilityCriteria.Internal
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
