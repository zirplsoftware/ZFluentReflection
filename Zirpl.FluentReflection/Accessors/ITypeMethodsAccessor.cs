using System;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    public interface ITypeMethodsAccessor
    {
        bool Exists { get; }

        MethodInfo[] MethodInfos { get; }
    }
}
