using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal class TypeCriteria : MemberInfoCriteriaBase
    {
        internal TypeCompatibilityCriteria CompatibilityCriteria { get; private set; }
        internal TypeNameCriteria NameCriteria { get; private set; }

        internal TypeCriteria(TypeSource typeSource)
        {
            NameCriteria = new TypeNameCriteria(typeSource);
            SubCriterias.Add(NameCriteria);
            CompatibilityCriteria = new TypeCompatibilityCriteria(typeSource);
            SubCriterias.Add(CompatibilityCriteria);
        }

        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            return memberInfos;
        }

        protected internal override bool ShouldRun
        {
            get
            {
                return false;
            }
        }
    }
}
