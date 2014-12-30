using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MethodReturnTypeCriteria :TypeCriteria
    {
        internal bool Void { get; set; }
        internal bool NotVoid { get; set; }

        public override bool IsMatch(MemberInfo memberInfo)
        {
            // TODO: do the null checks
            return base.IsMatch(((MethodInfo)memberInfo).ReturnType);
        }
    }
}
