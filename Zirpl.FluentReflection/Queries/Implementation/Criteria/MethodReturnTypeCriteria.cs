using System;
using System.Linq;
using System.Reflection;
using Zirpl.FluentReflection.Queries.implementation.criteria;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class MethodReturnTypeCriteria :TypeCriteria
    {
        internal bool Void { get; set; }
        internal bool NotVoid { get; set; }

        internal MethodReturnTypeCriteria()
            :base(TypeSource.MethodReturnType)
        {
        }

        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            return memberInfos.Where(o => IsMatch(o.GetAssociatedType(TypeSource.MethodReturnType))).ToArray();
        }

        private bool IsMatch(Type type)
        {
            if (type == null && NotVoid) return false;
            if (type != null && Void) return false;
            return true;
        }

        protected internal override bool ShouldRun
        {
            get
            {
                return Void
                       || NotVoid
                       || base.ShouldRun;
            }
        }
    }
}
