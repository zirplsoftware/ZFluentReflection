using System;
using System.Collections.Generic;

namespace Zirpl.FluentReflection.Queries
{
    public interface INameCriteriaBuilder
    {
        INameCriteriaBuilder Exactly(String name);
        INameCriteriaBuilder Any(IEnumerable<String> names);
        INameCriteriaBuilder ExactlyIgnoreCase(String name);
        INameCriteriaBuilder AnyIgnoreCase(IEnumerable<String> names);
        INameCriteriaBuilder StartingWith(String name);
        INameCriteriaBuilder StartingWithAny(IEnumerable<String> names);
        INameCriteriaBuilder StartingWithIgnoreCase(String name);
        INameCriteriaBuilder StartingWithAnyIgnoreCase(IEnumerable<String> names);
        INameCriteriaBuilder Containing(String name);
        INameCriteriaBuilder ContainingAny(IEnumerable<String> names);
        INameCriteriaBuilder ContainingIgnoreCase(String name);
        INameCriteriaBuilder ContainingAnyIgnoreCase(IEnumerable<String> names);
        INameCriteriaBuilder EndingWith(String name);
        INameCriteriaBuilder EndingWithAny(IEnumerable<String> names);
        INameCriteriaBuilder EndingWithIgnoreCase(String name);
        INameCriteriaBuilder EndingWithAnyIgnoreCase(IEnumerable<String> names);
    }
}
