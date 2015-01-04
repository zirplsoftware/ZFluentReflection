using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    public sealed class PropertyAccessor<T>
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly Object _obj;

        internal PropertyAccessor(PropertyInfo propertyInfo, Object obj)
        {
            _propertyInfo = propertyInfo;
            _obj = obj;
        }

        public bool Exists
        {
            get { return _propertyInfo != null; }
        }

        public T Value
        {
#if !PORTABLE
            get { return (T)_propertyInfo.GetValue(_obj); }
            set { _propertyInfo.SetValue(_obj, value); }
#else
            get { return (T)_propertyInfo.GetValue(_obj, null); }
            set { _propertyInfo.SetValue(_obj, value, null); }
#endif
        }

        public PropertyInfo PropertyInfo { get { return _propertyInfo;} }
    }
}
