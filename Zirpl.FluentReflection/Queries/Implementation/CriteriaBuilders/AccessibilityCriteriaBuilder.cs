namespace Zirpl.FluentReflection.Queries
{
    internal sealed class AccessibilityCriteriaBuilder :
        IAccessibilityCriteriaBuilder,
        ICSharpAccessibilityCriteriaBuilder
    {
        private readonly AccessibilityCriteria _memberAccessibilityCriteria;

        internal AccessibilityCriteriaBuilder(AccessibilityCriteria memberAccessibilityCriteria)
        {
            _memberAccessibilityCriteria = memberAccessibilityCriteria;
        }

        ICSharpAccessibilityCriteriaBuilder IAccessibilityCriteriaBuilder.CSharp()
        {
            return this;
        }

        ICSharpAccessibilityCriteriaBuilder ICSharpAccessibilityCriteriaBuilder.Public()
        {
            _memberAccessibilityCriteria.Public = true;
            return this;
        }

        ICSharpAccessibilityCriteriaBuilder ICSharpAccessibilityCriteriaBuilder.Private()
        {
            _memberAccessibilityCriteria.Private = true;
            return this;
        }

        ICSharpAccessibilityCriteriaBuilder ICSharpAccessibilityCriteriaBuilder.Protected()
        {
            _memberAccessibilityCriteria.Family = true;
            return this;
        }

        ICSharpAccessibilityCriteriaBuilder ICSharpAccessibilityCriteriaBuilder.Internal()
        {
            _memberAccessibilityCriteria.Assembly = true;
            return this;
        }

        ICSharpAccessibilityCriteriaBuilder ICSharpAccessibilityCriteriaBuilder.ProtectedOrInternal()
        {
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
            return this;
        }

        public void NotPrivate()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Family = true;
            _memberAccessibilityCriteria.Assembly = true;
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
        }

        public void NotPublic()
        {
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Family = true;
            _memberAccessibilityCriteria.Assembly = true;
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
        }

        public void All()
        {
            _memberAccessibilityCriteria.Public = true;
            _memberAccessibilityCriteria.Private = true;
            _memberAccessibilityCriteria.Family = true;
            _memberAccessibilityCriteria.Assembly = true;
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
        }
        
        IAccessibilityCriteriaBuilder IAccessibilityCriteriaBuilder.Public()
        {
            _memberAccessibilityCriteria.Public = true;
            return this;
        }

        IAccessibilityCriteriaBuilder IAccessibilityCriteriaBuilder.Private()
        {
            _memberAccessibilityCriteria.Private = true;
            return this;
        }

        IAccessibilityCriteriaBuilder IAccessibilityCriteriaBuilder.Family()
        {
            _memberAccessibilityCriteria.Family = true;
            return this;
        }

        IAccessibilityCriteriaBuilder IAccessibilityCriteriaBuilder.Assembly()
        {
            _memberAccessibilityCriteria.Assembly = true;
            return this;
        }

        IAccessibilityCriteriaBuilder IAccessibilityCriteriaBuilder.FamilyOrAssembly()
        {
            _memberAccessibilityCriteria.FamilyOrAssembly = true;
            return this;
        }
    }
}
