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

        public MemberInfo[] FilterMatches(MemberInfo[] memberInfos)
        {
            MemberInfo[] result = memberInfos;
            if (ShouldRunFilter)
            {
                result = DoFilterMatches(result);
            }
            // run the subfilters too
            foreach (var subFilter in SubFilters)
            {
                if (subFilter.ShouldRunFilter)
                {
                    result = subFilter.DoFilterMatches(result);
                }
            }
            return result;
        }

        protected abstract MemberInfo[] DoFilterMatches(MemberInfo[] memberInfos);

        protected internal abstract bool ShouldRunFilter { get; }
    }
}
