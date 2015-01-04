using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyCriteria : MemberInfoQueryCriteriaBase
    {
        internal TypeCriteria PropertyTypeCriteria { get; private set; }
        internal bool CanRead { get; set; }
        internal bool CanWrite { get; set; }

        internal PropertyCriteria()
        {
            PropertyTypeCriteria = new TypeCriteria(TypeSource.PropertyType);
            SubCriterias.Add(PropertyTypeCriteria);
        }

        private bool IsMatch(MemberInfo memberInfo)
        {
            var property = (PropertyInfo) memberInfo;
            if (!property.CanRead && CanRead) return false;
            if (!property.CanWrite && CanWrite) return false;
            return true;
        }

        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            return memberInfos.Where(IsMatch).ToArray();
        }

        protected internal override bool ShouldRun
        {
            get
            {
                return CanRead || CanWrite;
            }
        }
    }
}
