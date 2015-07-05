using System;
using System.Collections.Generic;
using Zirpl.FluentReflection.Queries.Implementation.Criteria;

namespace Zirpl.FluentReflection.Queries.Implementation.CriteriaBuilders
{
    internal sealed class TypeCompatibilityCriteriaBuilder : ITypeCompatibilityCriteriaBuilder
    {
        private readonly TypeCriteria _typeCriteria;

        internal TypeCompatibilityCriteriaBuilder(TypeCriteria typeCriteria)
        {
            _typeCriteria = typeCriteria;
        }
        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFrom(Type type)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = new[] { type };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFrom<T>()
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = new[] { typeof(T) };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFromAll(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = types;
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFromAny(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = types;
            _typeCriteria.AssignableFromAny = true;
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableTo(Type type)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = new[] { type };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableTo<T>()
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = new[] { typeof(T) };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableToAll(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = types;
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableToAny(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = types;
            _typeCriteria.AssignableToAny = true;
            return this;
        }
    }
}