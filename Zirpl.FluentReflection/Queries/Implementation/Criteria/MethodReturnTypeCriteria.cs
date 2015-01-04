using System;

namespace Zirpl.FluentReflection.Queries.Criteria
{
    internal sealed class MethodReturnTypeCriteria :TypeCriteria
    {
        internal bool Void { get; set; }
        internal bool NotVoid { get; set; }

        internal MethodReturnTypeCriteria()
            :base(TypeSource.MethodReturnType)
        {
        }

        protected override bool IsMatch(Type type)
        {
            if (type == null && NotVoid) return false;
            if (type != null && Void) return false;
            return base.IsMatch(type);
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
