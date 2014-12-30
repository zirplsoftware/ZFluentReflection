using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal sealed class FieldAccessor
    {
        private readonly FieldInfo _fieldInfo;

        internal FieldAccessor(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }
    }
}
