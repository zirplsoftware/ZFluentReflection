using System;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class MethodCriteria: MemberInfoQueryCriteriaBase
    {
        internal MethodReturnTypeCriteria ReturnTypeCriteria { get; private set; }
        internal Type[] ParameterTypes { get; set; }

        internal MethodCriteria()
        {
            ReturnTypeCriteria = new MethodReturnTypeCriteria();
            SubCriterias.Add(ReturnTypeCriteria);
        }

        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            if (ParameterTypes != null)
            {
                return memberInfos.Select(member => (MethodInfo)member).Where(method => 
                    method.GetParameters().Count() == ParameterTypes.Count()
                    && method.GetParameters().Select(
                        (parameter, index) => parameter.ParameterType == ParameterTypes[index]).Aggregate(
                            true, (a, b) => a && b)).Select(o => (MemberInfo)o).ToArray();
            }
            else
            {
                return memberInfos;
            }
        }

        protected internal override bool ShouldRun
        {
            get { return ParameterTypes != null; }
        }
    }
}
