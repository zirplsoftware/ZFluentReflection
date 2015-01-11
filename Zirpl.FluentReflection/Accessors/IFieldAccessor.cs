﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection.Accessors
{
    public interface IFieldAccessor<T> : ITypeFieldAccessor
    {
        T Value { get; set; }
    }
}