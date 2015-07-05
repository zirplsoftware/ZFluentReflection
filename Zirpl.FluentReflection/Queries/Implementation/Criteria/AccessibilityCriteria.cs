using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class AccessibilityCriteria : MemberInfoCriteriaBase
    {
        internal bool Public { get; set; }
        internal bool Private { get; set; }
        internal bool Family { get; set; }
        internal bool FamilyOrAssembly { get; set; }
        internal bool Assembly { get; set; }
        
        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            return memberInfos.Where(IsMatch).ToArray();
        }

        protected internal override bool ShouldRun
        {
            get
            {
                var countNotPublic = 0;
                countNotPublic += Private ? 1 : 0;
                countNotPublic += Family ? 1 : 0;
                countNotPublic += Assembly ? 1 : 0;
                countNotPublic += FamilyOrAssembly ? 1 : 0;

                // if 0 or 4, BindingFlags will have handled everything
                return countNotPublic != 0 && countNotPublic != 4;
            }
        }

        // ONLY internal for testing
        private bool IsMatch(MemberInfo memberInfo)
        {
            if (memberInfo == null) return false;

            var accessibility = memberInfo.GetAccessibility();
            if (accessibility == MemberInfoAccessibility.Public && !Public) return false;
            if (accessibility == MemberInfoAccessibility.Private && !Private) return false;
            if (accessibility == MemberInfoAccessibility.Family && !Family) return false;
            if (accessibility == MemberInfoAccessibility.Assembly && !Assembly) return false;
            if (accessibility == MemberInfoAccessibility.FamilyOrAssembly && !FamilyOrAssembly) return false;
            return true;
        }
    }
}
