using System;
using System.Collections.Generic;

namespace Zirpl.FluentReflection.Queries
{
    public interface ITypeCompatibilityCriteriaBuilder
    {
        ITypeCompatibilityCriteriaBuilder AssignableFrom(Type type);
        ITypeCompatibilityCriteriaBuilder AssignableFrom<T>();
        ITypeCompatibilityCriteriaBuilder AssignableFromAll(IEnumerable<Type> types);
        ITypeCompatibilityCriteriaBuilder AssignableFromAny(IEnumerable<Type> types);
        ITypeCompatibilityCriteriaBuilder AssignableTo(Type type);
        ITypeCompatibilityCriteriaBuilder AssignableTo<T>();
        ITypeCompatibilityCriteriaBuilder AssignableToAll(IEnumerable<Type> types);
        ITypeCompatibilityCriteriaBuilder AssignableToAny(IEnumerable<Type> types);
    }
}