using System;

namespace Zirpl.FluentReflection.Queries
{
    public interface ITypeQuery : IQueryResult<Type>, ICacheableQuery<Type>
    {
        ITypeQuery OfTypeCompatibility(Action<ITypeCompatibilityCriteriaBuilder> builder);
        ITypeQuery Named(Action<ITypeNameCriteriaBuilder> builder);
    }
}