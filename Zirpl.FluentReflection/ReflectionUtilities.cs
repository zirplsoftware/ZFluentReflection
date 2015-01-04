using System;
using System.ComponentModel;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    public static partial class ReflectionUtilities
    {
        public static void ClearCache()
        {
            CacheService.ClearCache();
            InstanceTypeAccessor.ClearCache();
        }

        public static Object GetStaticValue(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");

            return fieldInfo.GetValue(null);
        }

        public static void SetStaticValue(this FieldInfo fieldInfo, Object value)
        {
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");

            fieldInfo.SetValue(null, value);
        }

        public static Object GetStaticValue(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");

#if !PORTABLE
            return propertyInfo.GetValue(null);
#else
            return propertyInfo.GetValue(null, null);
#endif
        }

        public static void SetStaticValue(this PropertyInfo propertyInfo, Object value)
        {
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");

#if !PORTABLE
            propertyInfo.SetValue(null, value);
#else
            propertyInfo.SetValue(null, value, null);
#endif
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            return type.IsValueType || type.GetDefaultConstrtructor() != null;
        }

        public static ConstructorInfo GetDefaultConstrtructor(this Type type)
        {
            return type.GetConstructor(new Type[] {});
        }

        public static MemberInfoAccessibility GetAccessibility(this MemberInfo memberInfo)
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");

            if (memberInfo is MethodBase)
            {
                var method = (MethodBase)memberInfo;
                return GetMethodAccessibility(method);
            }
            else if (memberInfo is EventInfo)
            {
                var eventInfo = (EventInfo)memberInfo;
                var addMethod = eventInfo.GetAddMethod(true);
                var removeMethod = eventInfo.GetRemoveMethod(true);
                var addAccessibility = addMethod != null
                    ? GetMethodAccessibility(addMethod)
                    : MemberInfoAccessibility.Private;
                var removeAccessibility = removeMethod != null
                    ? GetMethodAccessibility(removeMethod)
                    : MemberInfoAccessibility.Private;
                return (MemberInfoAccessibility)Math.Min((int)addAccessibility, (int)removeAccessibility);
            }
            else if (memberInfo is FieldInfo)
            {
                var field = (FieldInfo)memberInfo;
                if (field.IsPublic) return MemberInfoAccessibility.Public;
                if (field.IsPrivate) return MemberInfoAccessibility.Private;
                if (field.IsFamily) return MemberInfoAccessibility.Family;
                if (field.IsAssembly) return MemberInfoAccessibility.Assembly;
                if (field.IsFamilyOrAssembly) return MemberInfoAccessibility.FamilyOrAssembly;
                throw new ArgumentException("Unexpected case- no accessibility found: " + field);
            }
            else if (memberInfo is PropertyInfo)
            {
                var propertyinfo = (PropertyInfo)memberInfo;
                var getMethod = propertyinfo.GetGetMethod(true);
                var setMethod = propertyinfo.GetSetMethod(true);
                var getAccessibility = getMethod != null
                    ? GetMethodAccessibility(getMethod)
                    : MemberInfoAccessibility.Private;
                var setAccessibility = setMethod != null
                    ? GetMethodAccessibility(setMethod)
                    : MemberInfoAccessibility.Private;
                return (MemberInfoAccessibility)Math.Min((int)getAccessibility, (int)setAccessibility);
            }
            else if (memberInfo is Type)
            {
                // nested types
                var type = (Type)memberInfo;
                if (type.IsNested)
                {
                    if (type.IsNestedPublic) return MemberInfoAccessibility.Public;
                    if (type.IsNestedPrivate) return MemberInfoAccessibility.Private;
                    if (type.IsNestedFamily) return MemberInfoAccessibility.Family;
                    if (type.IsNestedAssembly) return MemberInfoAccessibility.Assembly;
                    if (type.IsNestedFamORAssem) return MemberInfoAccessibility.FamilyOrAssembly;
                    throw new ArgumentException("Unexpected case- no accessibility found: " + type);
                }
                else
                {
                    if (type.IsPublic) return MemberInfoAccessibility.Public;
                    return MemberInfoAccessibility.Public;
                    //if (!type.IsPublic) return MemberInfoAccessibility.Family;
                }
            }
            else
            {
                throw new Exception("Unexpected MemberInfo type: " + memberInfo.GetType().ToString());
            }
        }

        private static MemberInfoAccessibility GetMethodAccessibility(MethodBase method)
        {
            if (method.IsPublic) return MemberInfoAccessibility.Public;
            if (method.IsPrivate) return MemberInfoAccessibility.Private;
            if (method.IsFamily) return MemberInfoAccessibility.Family;
            if (method.IsAssembly) return MemberInfoAccessibility.Assembly;
            if (method.IsFamilyOrAssembly) return MemberInfoAccessibility.FamilyOrAssembly;
            throw new ArgumentException("Unexpected case- no accessibility found: " + method);
        }
    }
}
