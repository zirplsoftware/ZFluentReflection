﻿using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyReadWriteEvaluator : IMatchEvaluator
    {
        internal bool CanRead { get; set; }
        internal bool CanWrite { get; set; }

        public bool IsMatch(MemberInfo memberInfo)
        {
            var property = (PropertyInfo) memberInfo;
            if (!property.CanRead && CanRead) return false;
            if (!property.CanWrite && CanWrite) return false;
            return true;
        }
    }
}