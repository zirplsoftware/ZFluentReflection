using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    public sealed class FieldAccessor<T>
    {
        private readonly FieldInfo _fieldInfo;
        private readonly Object _obj;

        internal FieldAccessor(FieldInfo fieldInfo, Object obj)
        {
            _fieldInfo = fieldInfo;
            _obj = obj;
        }

        public bool Exists
        {
            get { return _fieldInfo != null; }
        }

        public T Value
        {
            get { return (T)_fieldInfo.GetValue(_obj); }
            set { _fieldInfo.SetValue(_obj, value); }
        }

        public FieldInfo FieldInfo { get { return _fieldInfo; } }
    }
}
