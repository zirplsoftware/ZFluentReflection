using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MethodReturnTypeCriteria :TypeCriteria
    {
        internal bool Void { get; set; }
        internal bool NotVoid { get; set; }
        
        protected override Type GetTypeToCheck(MemberInfo memberInfo)
        {
            return ((MethodInfo)memberInfo).ReturnType;
        }

        protected override bool IsMatch(Type type)
        {
            if (type == null && NotVoid) return false;
            if (type != null && Void) return false;
            return base.IsMatch(type);
        }

        protected override bool ShouldRunFilter
        {
            get
            {
                return Void
                       || NotVoid
                       || base.ShouldRunFilter;
            }
        }
    }
}
