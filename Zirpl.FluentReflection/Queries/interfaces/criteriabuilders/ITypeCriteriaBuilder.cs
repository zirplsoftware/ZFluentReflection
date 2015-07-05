using System;

namespace Zirpl.FluentReflection.Queries
{
    public interface ITypeCriteriaBuilder
    {
        ITypeCriteriaBuilder OfTypeCompatibility(Action<ITypeCompatibilityCriteriaBuilder> builder);
        ITypeCriteriaBuilder Named(Action<ITypeNameCriteriaBuilder> builder);
    }
}