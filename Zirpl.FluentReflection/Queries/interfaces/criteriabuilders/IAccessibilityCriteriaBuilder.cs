namespace Zirpl.FluentReflection.Queries
{
    public interface IAccessibilityCriteriaBuilder
    {
        ICSharpAccessibilityCriteriaBuilder CSharp();
        IAccessibilityCriteriaBuilder Public();
        IAccessibilityCriteriaBuilder Private();
        IAccessibilityCriteriaBuilder Family();
        IAccessibilityCriteriaBuilder Assembly();
        IAccessibilityCriteriaBuilder FamilyOrAssembly();
        void NotPrivate();
        void NotPublic();
        void All();
    }
}