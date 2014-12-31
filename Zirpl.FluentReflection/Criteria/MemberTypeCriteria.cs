using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberTypeCriteria : MemberInfoQueryCriteriaBase
    {
        internal bool Constructor { get; set; }
        internal bool Event { get; set; }
        internal bool Field { get; set; }
        internal bool Method { get; set; }
        internal bool NestedType { get; set; }
        internal bool Property { get; set; }
        
        protected override MemberInfo[] DoFilterMatches(MemberInfo[] memberInfos)
        {
            throw new NotImplementedException();
        }

        protected internal override bool ShouldRunFilter
        {
            get
            {
                // right now this is FULLY handled by the service, so we can ignore here... still keeping around because the MemberTypesBuilder uses it
                return false;
            }
        }
    }
}
