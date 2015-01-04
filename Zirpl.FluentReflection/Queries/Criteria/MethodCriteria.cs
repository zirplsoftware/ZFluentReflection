using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal sealed class MethodCriteria: MemberInfoQueryCriteriaBase
    {
        internal MethodReturnTypeCriteria ReturnTypeCriteria { get; private set; }

        internal MethodCriteria()
        {
            ReturnTypeCriteria = new MethodReturnTypeCriteria();
            SubCriterias.Add(ReturnTypeCriteria);
        }

        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            return memberInfos;
        }

        protected internal override bool ShouldRun
        {
            get { return false; }
        }
    }
}
