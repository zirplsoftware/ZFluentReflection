using System;
using System.Collections.Generic;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class TypeCompatibilityCriteriaBuilder : ITypeCompatibilityCriteriaBuilder
    {
        private readonly TypeCompatibilityCriteria _typeCompatibilityCriteria;

        internal TypeCompatibilityCriteriaBuilder(TypeCompatibilityCriteria typeCompatibilityCriteria)
        {
            _typeCompatibilityCriteria = typeCompatibilityCriteria;
        }
        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFrom(Type type)
        {
            if (_typeCompatibilityCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableFroms = new[] { type };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFrom<T>()
        {
            if (_typeCompatibilityCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableFroms = new[] { typeof(T) };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFromAll(IEnumerable<Type> types)
        {
            if (_typeCompatibilityCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableFroms = types;
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableFromAny(IEnumerable<Type> types)
        {
            if (_typeCompatibilityCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableFroms = types;
            _typeCompatibilityCriteria.AssignableFromAny = true;
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableTo(Type type)
        {
            if (_typeCompatibilityCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableTos = new[] { type };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableTo<T>()
        {
            if (_typeCompatibilityCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableTos = new[] { typeof(T) };
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableToAll(IEnumerable<Type> types)
        {
            if (_typeCompatibilityCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableTos = types;
            return this;
        }

        ITypeCompatibilityCriteriaBuilder ITypeCompatibilityCriteriaBuilder.AssignableToAny(IEnumerable<Type> types)
        {
            if (_typeCompatibilityCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCompatibilityCriteria.AssignableTos = types;
            _typeCompatibilityCriteria.AssignableToAny = true;
            return this;
        }
    }
}