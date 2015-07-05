namespace Zirpl.FluentReflection.Queries
{
    public interface ICSharpAccessibilityCriteriaBuilder
    {
        ICSharpAccessibilityCriteriaBuilder Public();
        ICSharpAccessibilityCriteriaBuilder Private();
        ICSharpAccessibilityCriteriaBuilder Protected();
        ICSharpAccessibilityCriteriaBuilder Internal();
        ICSharpAccessibilityCriteriaBuilder ProtectedOrInternal();
        void NotPrivate();
        void NotPublic();
        void All();
    }
}