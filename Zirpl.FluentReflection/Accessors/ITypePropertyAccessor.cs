using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection.Accessors
{
    public interface ITypePropertyAccessor
    {
        bool Exists { get; }
        PropertyInfo PropertyInfo { get; }
    }
}
