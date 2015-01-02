using System;
using System.ComponentModel;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    public static class ReflectionUtilities
    {
        public static void ClearCache()
        {
            CacheService.ClearCache();
            InstanceTypeAccessor.ClearCache();
        }

        public static PropertyAccessor<T> Property<T>(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new PropertyAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).Property(name), obj);
        }

        public static PropertyAccessor<Object> Property(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new PropertyAccessor<Object>(InstanceTypeAccessor.Get(obj.GetType()).Property(name), obj);
        }

        public static FieldAccessor<T> Field<T>(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new FieldAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).Field(name), obj);
        }

        public static FieldAccessor<Object> Field(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new FieldAccessor<Object>(InstanceTypeAccessor.Get(obj.GetType()).Field(name), obj);
        }

        public static EventAccessor<T> Event<T>(this Object obj, String name)
            where T: EventArgs
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new EventAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).Event(name), obj);
        }

        public static EventAccessor<EventArgs> Event(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new EventAccessor<EventArgs>(InstanceTypeAccessor.Get(obj.GetType()).Event(name), obj);
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
            bool hasDefaultConstructor = type.IsValueType
                                         || type.GetConstructor(new Type[] { }) != null;

            return hasDefaultConstructor;
        }
    }
}
