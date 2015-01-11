using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection.Accessors
{
    internal class TypeFieldAccessor : ITypeFieldAccessor
    {
        private readonly FieldInfo _fieldInfo;

        internal TypeFieldAccessor(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public bool Exists
        {
            get { return _fieldInfo != null; }
        }

        public FieldInfo FieldInfo { get { return _fieldInfo; } }
    }
}
