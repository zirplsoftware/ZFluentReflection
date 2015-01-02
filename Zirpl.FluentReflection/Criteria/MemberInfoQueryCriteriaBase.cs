using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal abstract class MemberInfoQueryCriteriaBase : IMemberInfoQueryCriteria
    {
        protected IList<MemberInfoQueryCriteriaBase> SubCriterias { get; private set; }

        internal MemberInfoQueryCriteriaBase()
        {
            SubCriterias = new List<MemberInfoQueryCriteriaBase>();
        }

        public MemberInfo[] GetMatches(MemberInfo[] memberInfos)
        {
            MemberInfo[] result = memberInfos;
            if (ShouldRun)
            {
                result = RunGetMatches(result);
            }
            // run the subfilters too
            return SubCriterias.Where(subCriteria => subCriteria.ShouldRun).Aggregate(result, (current, subCriteria) => subCriteria.RunGetMatches(current));
        }

        protected abstract MemberInfo[] RunGetMatches(MemberInfo[] memberInfos);

        protected internal abstract bool ShouldRun { get; }
    }
}
