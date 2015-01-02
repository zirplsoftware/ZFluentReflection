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
        protected IList<MemberInfoQueryCriteriaBase> SubFilters { get; private set; }

        internal MemberInfoQueryCriteriaBase()
        {
            SubFilters = new List<MemberInfoQueryCriteriaBase>();
        }

        public MemberInfo[] GetMatches(MemberInfo[] memberInfos)
        {
            MemberInfo[] result = memberInfos;
            if (ShouldRunFilter)
            {
                result = DoGetMatches(result);
            }
            // run the subfilters too
            foreach (var subFilter in SubFilters)
            {
                if (subFilter.ShouldRunFilter)
                {
                    result = subFilter.DoGetMatches(result);
                }
            }
            return result;
        }

        protected abstract MemberInfo[] DoGetMatches(MemberInfo[] memberInfos);

        protected internal abstract bool ShouldRunFilter { get; }
    }
}
