using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    internal class TypePropertyAccessor : ITypePropertyAccessor
    {
        private readonly PropertyInfo _propertyInfo;

        internal TypePropertyAccessor(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public bool Exists
        {
            get { return _propertyInfo != null; }
        }
        
        public PropertyInfo PropertyInfo { get { return _propertyInfo;} }
    }
}
