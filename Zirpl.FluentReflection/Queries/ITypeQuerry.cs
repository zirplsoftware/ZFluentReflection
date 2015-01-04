﻿using System;
using System.Collections.Generic;

namespace Zirpl.FluentReflection.Queries
{
    public interface ITypeQuery : IQueryResult<Type>, ICacheableQuery<Type>
    {
        ITypeQuery AssignableFrom(Type type);
        ITypeQuery AssignableFrom<T>();
        ITypeQuery AssignableFromAll(IEnumerable<Type> types);
        ITypeQuery AssignableFromAny(IEnumerable<Type> types);
        ITypeQuery AssignableTo(Type type);
        ITypeQuery AssignableTo<T>();
        ITypeQuery AssignableToAll(IEnumerable<Type> types);
        ITypeQuery AssignableToAny(IEnumerable<Type> types);
        ITypeNameSubQuery<Type, ITypeQuery> Named();
    }
}