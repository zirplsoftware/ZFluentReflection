using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    internal sealed class FieldAccessor<T> : TypeFieldAccessor, IFieldAccessor<T>
    {
        private readonly Object _obj;

        internal FieldAccessor(FieldInfo fieldInfo, Object obj)
            :base(fieldInfo)
        {
            _obj = obj;
        }

        public T Value
        {
            get { return (T)FieldInfo.GetValue(_obj); }
            set { FieldInfo.SetValue(_obj, value); }
        }
    }
}
