using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection.Accessors
{
    public interface IMethodAccessor<T> : ITypeMethodsAccessor
    {
        T Invoke(params Object[] args);
    }
}
