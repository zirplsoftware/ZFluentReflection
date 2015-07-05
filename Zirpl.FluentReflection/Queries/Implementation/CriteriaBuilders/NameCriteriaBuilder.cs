using System;
using System.Collections.Generic;
using Zirpl.FluentReflection.Queries.Implementation.Criteria;

namespace Zirpl.FluentReflection.Queries.Implementation.CriteriaBuilders
{
    internal class NameCriteriaBuilder : INameCriteriaBuilder
    {
        private readonly NameCriteria _nameCriteria;

        internal NameCriteriaBuilder(NameCriteria nameCriteria)
        {
            _nameCriteria = nameCriteria;
        }

        INameCriteriaBuilder INameCriteriaBuilder.Exactly(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.Any(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.ExactlyIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.IgnoreCase = true;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.AnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.IgnoreCase = true;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.StartingWith(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.StartingWithAny(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.StartingWithIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            _nameCriteria.IgnoreCase = true;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.StartingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            _nameCriteria.IgnoreCase = true;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.Containing(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.ContainingAny(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.ContainingIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            _nameCriteria.IgnoreCase = true;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.ContainingAnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            _nameCriteria.IgnoreCase = true;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.EndingWith(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.EndingWithAny(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.EndingWithIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            _nameCriteria.IgnoreCase = true;
            return this;
        }

        INameCriteriaBuilder INameCriteriaBuilder.EndingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            _nameCriteria.IgnoreCase = true;
            return this;
        }
    }
}
