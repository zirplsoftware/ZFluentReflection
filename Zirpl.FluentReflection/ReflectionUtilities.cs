﻿using System;
using System.ComponentModel;
using System.Reflection;
using Zirpl.FluentReflection.Accessors;
using Zirpl.FluentReflection.Dynamic;
using Zirpl.FluentReflection.Queries;

namespace Zirpl.FluentReflection
{
    public static class ReflectionUtilities
    {
        public static void ClearCache()
        {
            CacheService.ClearCache();
            InstanceTypeAccessor.ClearCache();
        }
        public static ITypeQuery QueryTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            return new TypeQuery(assembly);
        }

#if !PORTABLE
        public static ITypeQuery QueryTypes(this AppDomain appDomain)
        {
            if (appDomain == null) throw new ArgumentNullException("appDomain");
            return new TypeQuery(appDomain);
        }
#endif

        public static IPropertyQuery QueryProperties(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return new PropertyQuery(type);
        }

        public static IFieldQuery QueryFields(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return new FieldQuery(type);
        }

        public static IMethodQuery QueryMethods(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return new MethodQuery(type);
        }

        public static IConstructorQuery QueryConstructors(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return new ConstructorQuery(type);
        }

        public static INestedTypeQuery QueryNestedTypes(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return new NestedTypeQuery(type);
        }

        public static IEventQuery QueryEvents(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return new EventQuery(type);
        }

        public static IMemberQuery QueryMembers(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return new MemberQuery(type);
        }

        public static dynamic ToDynamic<T>(this T value)
        {
            return new DynamicWrapper<T>(ref value);
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

        public static IPropertyAccessor<T> Property<T>(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new PropertyAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).PropertyInfo(name), obj);
        }

        public static IPropertyAccessor<Object> Property(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new PropertyAccessor<Object>(InstanceTypeAccessor.Get(obj.GetType()).PropertyInfo(name), obj);
        }

        public static PropertyInfo PropertyInfo(this Type type, String name)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return InstanceTypeAccessor.Get(type).PropertyInfo(name);
        }

        public static IMethodAccessor<T> Method<T>(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new MethodsAccessor<T>(name, obj);
        }

        public static IMethodAccessor<Object> Method(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new MethodsAccessor<Object>(name, obj);
        }

        public static MethodInfo[] MethodInfos(this Type type, String name)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new TypeMethodsAccessor(name, type).MethodInfos;
        }

        public static IFieldAccessor<T> Field<T>(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new FieldAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).FieldInfo(name), obj);
        }

        public static IFieldAccessor<Object> Field(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new FieldAccessor<Object>(InstanceTypeAccessor.Get(obj.GetType()).FieldInfo(name), obj);
        }

        public static FieldInfo FieldInfo(this Type type, String name)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return InstanceTypeAccessor.Get(type).FieldInfo(name);
        }

        public static EventAccessor<T> Event<T>(this Object obj, String name)
            where T : EventArgs
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new EventAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).EventInfo(name), obj);
        }

        public static EventAccessor<EventArgs> Event(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new EventAccessor<EventArgs>(InstanceTypeAccessor.Get(obj.GetType()).EventInfo(name), obj);
        }

        public static EventInfo EventInfo(this Type type, String name)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return InstanceTypeAccessor.Get(type).EventInfo(name);
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

            var methodBase = memberInfo as MethodBase;
            if (methodBase != null)
            {
                return GetMethodAccessibility(methodBase);
            }

            var eventInfo = memberInfo as EventInfo;
            if (eventInfo != null)
            {
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

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                if (fieldInfo.IsPublic) return MemberInfoAccessibility.Public;
                if (fieldInfo.IsPrivate) return MemberInfoAccessibility.Private;
                if (fieldInfo.IsFamily) return MemberInfoAccessibility.Family;
                if (fieldInfo.IsAssembly) return MemberInfoAccessibility.Assembly;
                if (fieldInfo.IsFamilyOrAssembly) return MemberInfoAccessibility.FamilyOrAssembly;
                throw new ArgumentException("Unexpected case- no accessibility found: " + fieldInfo);
            }

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                var getMethod = propertyInfo.GetGetMethod(true);
                var setMethod = propertyInfo.GetSetMethod(true);
                var getAccessibility = getMethod != null
                    ? GetMethodAccessibility(getMethod)
                    : MemberInfoAccessibility.Private;
                var setAccessibility = setMethod != null
                    ? GetMethodAccessibility(setMethod)
                    : MemberInfoAccessibility.Private;
                return (MemberInfoAccessibility)Math.Min((int)getAccessibility, (int)setAccessibility);
            }

            var type = memberInfo as Type;
            if (type != null)
            {
                // nested types
                if (type.IsNested)
                {
                    if (type.IsNestedPublic) return MemberInfoAccessibility.Public;
                    if (type.IsNestedPrivate) return MemberInfoAccessibility.Private;
                    if (type.IsNestedFamily) return MemberInfoAccessibility.Family;
                    if (type.IsNestedAssembly) return MemberInfoAccessibility.Assembly;
                    if (type.IsNestedFamORAssem) return MemberInfoAccessibility.FamilyOrAssembly;
                    throw new ArgumentException("Unexpected case- no accessibility found: " + type);
                }

                if (type.IsPublic) return MemberInfoAccessibility.Public;
                return MemberInfoAccessibility.Public;
                //if (!type.IsPublic) return MemberInfoAccessibility.Family;
            }

            throw new Exception("Unexpected MemberInfo type: " + memberInfo.GetType().ToString());
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
