using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class TypeFullNameEvaluator : NameEvaluator
    {
        protected override string GetNameToCheck(MemberInfo memberInfo)
        {
            var type = (Type) memberInfo;
            return type.FullName;
        }
    }
}