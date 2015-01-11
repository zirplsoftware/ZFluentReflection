using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    internal sealed class PropertyAccessor<T> : TypePropertyAccessor, IPropertyAccessor<T>
    {
        private readonly Object _obj;

        internal PropertyAccessor(PropertyInfo propertyInfo, Object obj)
            :base(propertyInfo)
        {
            _obj = obj;
        }

        public T Value
        {
#if !PORTABLE
            get { return (T)PropertyInfo.GetValue(_obj); }
            set { PropertyInfo.SetValue(_obj, value); }
#else
            get { return (T)PropertyInfo.GetValue(_obj, null); }
            set { PropertyInfo.SetValue(_obj, value, null); }
#endif
        }
    }
}
