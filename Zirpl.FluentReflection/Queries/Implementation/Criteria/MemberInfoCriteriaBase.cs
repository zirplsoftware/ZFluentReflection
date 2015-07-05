using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal abstract class MemberInfoCriteriaBase : IMemberInfoCriteria
    {
        protected IList<MemberInfoCriteriaBase> SubCriterias { get; private set; }

        internal MemberInfoCriteriaBase()
        {
            SubCriterias = new List<MemberInfoCriteriaBase>();
            SubCriterias.Add(this);
        }

        public MemberInfo[] GetMatches(MemberInfo[] memberInfos)
        {
            return SubCriterias.Where(subCriteria => subCriteria.ShouldRun).Aggregate(memberInfos, (current, subCriteria) => subCriteria.RunGetMatches(current));
        }

        protected abstract MemberInfo[] RunGetMatches(MemberInfo[] memberInfos);

        protected internal abstract bool ShouldRun { get; }
    }
}
