using System;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberAccessibilityCriteria : MemberInfoQueryCriteriaBase
    {
        internal bool Public { get; set; }
        internal bool Private { get; set; }
        internal bool Protected { get; set; }
        internal bool ProtectedInternal { get; set; }
        internal bool Internal { get; set; }
        
        protected override MemberInfo[] DoFilterMatches(MemberInfo[] memberInfos)
        {
            return memberInfos.Where(IsMatch).ToArray();
        }

        protected internal override bool ShouldRunFilter
        {
            get
            {
                var countNotPublic = 0;
                countNotPublic += Private ? 1 : 0;
                countNotPublic += Protected ? 1 : 0;
                countNotPublic += Internal ? 1 : 0;
                countNotPublic += ProtectedInternal ? 1 : 0;

                // if 0 or 4, BindingFlags will have handled everything
                return countNotPublic != 0 && countNotPublic != 4;
            }
        }

        private bool IsMatch(MemberInfo memberInfo)
        {
            // PUBLIC: DON'T bother checking ANYTHING public as that is COMPLETELY handled by the binding flags
            if (memberInfo is MethodBase)
            {
                var method = (MethodBase)memberInfo;
                if (!IsMethodMatch(method)) return false;
            }
            else if (memberInfo is EventInfo)
            {
                var eventInfo = (EventInfo)memberInfo;
                var addMethod = eventInfo.GetAddMethod(true);
                var removeMethod = eventInfo.GetRemoveMethod(true);
                if (!IsMethodMatch(addMethod) && !IsMethodMatch(removeMethod)) return false;
            }
            else if (memberInfo is FieldInfo)
            {
                var field = (FieldInfo)memberInfo;
                // presence here alone is enough to pass a Public
                if (field.IsPublic) return true;

                if (field.IsPrivate && !Private) return false;
                if (field.IsFamily && !Protected) return false;
                if (field.IsFamilyAndAssembly && !ProtectedInternal) return false;
                if (field.IsAssembly && !Internal) return false;
            }
            else if (memberInfo is PropertyInfo)
            {
                var propertyinfo = (PropertyInfo)memberInfo;
                var getMethod = propertyinfo.GetGetMethod(true);
                var setMethod = propertyinfo.GetSetMethod(true);
                if (!IsMethodMatch(getMethod) && !IsMethodMatch(setMethod)) return false;
            }
            else if (memberInfo is Type)
            {
                // nested types
                var type = (Type)memberInfo;
                // presence here alone is enough to pass a Public
                if (type.IsPublic) return true;

                if (type.IsNestedPrivate && !Private) return false;
                if (type.IsNestedFamily && !Protected) return false;
                if (type.IsNestedFamANDAssem && !ProtectedInternal) return false;
                if (type.IsNestedAssembly && !Internal) return false;
            }
            else
            {
                throw new Exception("Unexpected MemberInfo type: " +  memberInfo.GetType().ToString());
            }
            return true;
        }

        private bool IsMethodMatch(MethodBase method)
        {
            if (method == null) return false;
            // presence here alone is enough to pass a Public
            if (method.IsPublic) return true;

            if (method.IsPrivate && !Private) return false;
            if (method.IsFamily && !Protected) return false;
            if (method.IsFamilyAndAssembly && !ProtectedInternal) return false;
            if (method.IsAssembly && !Internal) return false;
            return true;
        }

    }
}
