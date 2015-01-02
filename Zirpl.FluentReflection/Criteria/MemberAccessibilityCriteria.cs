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
        
        protected override MemberInfo[] DoGetMatches(MemberInfo[] memberInfos)
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

        // ONLY internal for testing
        private bool IsMatch(MemberInfo memberInfo)
        {
            if (memberInfo == null) return false;
            
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
                if (field.IsPublic && !Public) return false;
                if (field.IsPrivate && !Private) return false;
                if (field.IsFamily && !Protected) return false;
                if (field.IsAssembly && !Internal) return false;
                if (field.IsFamilyOrAssembly && !ProtectedInternal) return false;
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
                if (type.IsNestedPublic && !Public) return false;
                if (type.IsNestedPrivate && !Private) return false;
                if (type.IsNestedFamily && !Protected) return false;
                if (type.IsNestedAssembly && !Internal) return false;
                if (type.IsNestedFamORAssem && !ProtectedInternal) return false;
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
            if (method.IsPublic && !Public) return false;
            if (method.IsPrivate && !Private) return false;
            if (method.IsFamily && !Protected) return false;
            if (method.IsAssembly && !Internal) return false;
            if (method.IsFamilyOrAssembly && !ProtectedInternal) return false;
            return true;
        }

    }
}
