using Zirpl.FluentReflection.Queries.Implementation.Criteria;

namespace Zirpl.FluentReflection.Queries.Implementation.CriteriaBuilders
{
    internal sealed class TypeNameCriteriaBuilder : NameCriteriaBuilder, ITypeNameCriteriaBuilder
    {
        private readonly TypeNameCriteria _typeNameCriteria;

        internal TypeNameCriteriaBuilder(TypeNameCriteria typeNameCriteria)
            :base(typeNameCriteria)
        {
            _typeNameCriteria = typeNameCriteria;
        }

        INameCriteriaBuilder ITypeNameCriteriaBuilder.Full()
        {
            _typeNameCriteria.UseFullName = true;
            return this;
        }
    }
}
