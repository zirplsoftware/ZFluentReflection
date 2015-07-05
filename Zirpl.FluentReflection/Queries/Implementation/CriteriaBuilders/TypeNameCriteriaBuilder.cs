namespace Zirpl.FluentReflection.Queries
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
