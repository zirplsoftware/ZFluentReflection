namespace Zirpl.FluentReflection.Queries
{
    public interface IMemberTypeCriteriaBuilder
    {
        IMemberTypeCriteriaBuilder Constructor();
        IMemberTypeCriteriaBuilder Event();
        IMemberTypeCriteriaBuilder Field();
        IMemberTypeCriteriaBuilder Method();
        IMemberTypeCriteriaBuilder NestedType();
        IMemberTypeCriteriaBuilder Property();
        void All();
    }
}