using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberTypeEvaluator : IMatchEvaluator
    {
        internal bool Constructor { get; set; }
        internal bool Event { get; set; }
        internal bool Field { get; set; }
        internal bool Method { get; set; }
        internal bool NestedType { get; set; }
        internal bool Property { get; set; }

        public bool IsMatch(MemberInfo memberInfo)
        {
            //if (Constructor
            //    || Event
            //    || Field
            //    || Method
            //    || NestedType
            //    || Property)
            //{
            //    if (memberInfo is ConstructorInfo && !Constructor) return false;
            //    if (memberInfo is EventInfo && !Event) return false;
            //    if (memberInfo is FieldInfo && !Field) return false;
            //    if (memberInfo is MethodInfo && !Method) return false;
            //    if (memberInfo is Type && !NestedType) return false;
            //    if (memberInfo is PropertyInfo && !Property) return false;
            //}
            // otherwise defaults to all, so it passes
            return true;
        }

        public bool IsMatchCheckRequired()
        {
            // right now this is FULLY handled by the service, so we can ignore here... still keeping around because the MemberTypesBuilder uses it
            return false;
        }
    }
}
