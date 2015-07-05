namespace Zirpl.FluentReflection.Queries
{
    public interface IScopeCriteriaBuilder
    {
        IScopeCriteriaBuilder Instance();
        IScopeCriteriaBuilder Static();
        IScopeCriteriaBuilder DeclaredOnThisType();
        IScopeCriteriaBuilder DeclaredOnBaseTypes();
        void All();
        void Default();
    }
}