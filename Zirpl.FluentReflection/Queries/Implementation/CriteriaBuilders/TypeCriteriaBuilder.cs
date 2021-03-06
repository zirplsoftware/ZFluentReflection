﻿using System;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class TypeCriteriaBuilder : ITypeCriteriaBuilder
    {
        private readonly TypeCriteria _typeCriteria;

        internal TypeCriteriaBuilder(TypeCriteria typeCriteria)
        {
            _typeCriteria = typeCriteria;
        }

        ITypeCriteriaBuilder ITypeCriteriaBuilder.OfTypeCompatibility(Action<ITypeCompatibilityCriteriaBuilder> builder)
        {
            builder(new TypeCompatibilityCriteriaBuilder(_typeCriteria.CompatibilityCriteria));
            return this;
        }

        ITypeCriteriaBuilder ITypeCriteriaBuilder.Named(Action<ITypeNameCriteriaBuilder> builder)
        {
            builder(new TypeNameCriteriaBuilder(_typeCriteria.NameCriteria));
            return this;
        }
    }
}
