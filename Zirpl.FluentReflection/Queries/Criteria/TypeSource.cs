using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal enum TypeSource
    {
        Self = 0,
        EventHandlerType = 1,
        FieldType = 2,
        MethodReturnType = 3,
        PropertyType = 4
    }
}
