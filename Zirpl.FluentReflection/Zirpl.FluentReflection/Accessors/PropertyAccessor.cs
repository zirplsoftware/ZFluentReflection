using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal sealed class PropertyAccessor
    {
        private readonly PropertyInfo _propertyInfo;

        internal PropertyAccessor(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }
    }
}
