using System;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberScopeCriteria : MemberInfoQueryCriteriaBase
    {
        private readonly Type _reflectedType;
        internal bool Instance { get; set; }
        internal bool Static { get; set; }
        internal bool DeclaredOnThisType { get; set; }
        internal bool DeclaredOnBaseTypes { get; set; }
        // TODO: implement including hiddenBySignature members, and exclude them otherwise

        internal MemberScopeCriteria(Type type)
        {
            _reflectedType = type;
        }

        protected override MemberInfo[] RunGetMatches(MemberInfo[] memberInfos)
        {
            return memberInfos.Where(IsMatch).ToArray();
        }

        protected internal override bool ShouldRun
        {
            get
            {
                // we can skip checks if 
                // 1) neither Type scope was chosen (in which case the default will be used)
                // 2) BOTH were chosen, but no depth
                // - STATIC vs INSTANCE is completely handled by the binding flags
                var canSkip = (!DeclaredOnThisType && !DeclaredOnBaseTypes)
                            || (DeclaredOnThisType && DeclaredOnBaseTypes);
                return !canSkip;
            }
        }

        private bool IsMatch(MemberInfo memberInfo)
        {
            if (memberInfo is MethodBase)
            {
                var method = (MethodBase)memberInfo;
                if (!IsDeclaredTypeMatch(method)) return false;
            }
            else if (memberInfo is EventInfo)
            {
                var eventInfo = (EventInfo)memberInfo;
                var addMethod = eventInfo.GetAddMethod(true);
                var removeMethod = eventInfo.GetRemoveMethod(true);
                if (!IsDeclaredTypeMatch(addMethod) && !IsDeclaredTypeMatch(removeMethod)) return false;
            }
            else if (memberInfo is FieldInfo)
            {
                var field = (FieldInfo)memberInfo;
                if (!IsDeclaredTypeMatch(field)) return false;
            }
            else if (memberInfo is PropertyInfo)
            {
                var propertyinfo = (PropertyInfo)memberInfo;
                var getMethod = propertyinfo.GetGetMethod(true);
                var setMethod = propertyinfo.GetSetMethod(true);
                if (!IsDeclaredTypeMatch(getMethod) && !IsDeclaredTypeMatch(setMethod)) return false;
            }
            else if (memberInfo is Type)
            {
                // nested types
                var type = (Type)memberInfo;
                if (!IsDeclaredTypeMatch(memberInfo)) return false;
            }
            else
            {
                throw new Exception("Unexpected MemberInfo type: " + memberInfo.GetType().ToString());
            }
            return true;
        }

        private bool IsDeclaredTypeMatch(MemberInfo memberInfo)
        {
            // no need for this check, since getting here means we need to check
            if (memberInfo.DeclaringType.Equals(_reflectedType) && !DeclaredOnThisType) return false;
            if (!memberInfo.DeclaringType.Equals(_reflectedType) && !DeclaredOnBaseTypes) return false;
            // if neither was chosen, then evaluate to true
            return true;
        }
    }
}
