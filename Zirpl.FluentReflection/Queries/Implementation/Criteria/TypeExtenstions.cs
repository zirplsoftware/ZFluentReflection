using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal static class TypeExtenstions
    {
        internal static Type GetAssociatedType(this MemberInfo memberInfo, TypeSource typeSource)
        {
            switch (typeSource)
            {
                case TypeSource.Self:
                    return (Type)memberInfo;
                case TypeSource.EventHandlerType:
                    return ((EventInfo)memberInfo).EventHandlerType;
                case TypeSource.FieldType:
                    return ((FieldInfo)memberInfo).FieldType;
                case TypeSource.MethodReturnType:
                    return ((MethodInfo)memberInfo).ReturnType;
                case TypeSource.PropertyType:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new Exception("Unknown TypeSource: " + typeSource);
            }
        }
    }
}
