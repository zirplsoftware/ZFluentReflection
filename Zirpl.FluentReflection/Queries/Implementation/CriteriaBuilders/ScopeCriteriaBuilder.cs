using System;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class ScopeCriteriaBuilder : IScopeCriteriaBuilder
    {
        private readonly ScopeCriteria _memberScopeCriteria;

        internal ScopeCriteriaBuilder(ScopeCriteria memberScopeCriteria)
        {
            _memberScopeCriteria = memberScopeCriteria;
        }

        IScopeCriteriaBuilder IScopeCriteriaBuilder.Instance()
        {
            _memberScopeCriteria.Instance = true;
            return this;
        }

        IScopeCriteriaBuilder IScopeCriteriaBuilder.Static()
        {
            _memberScopeCriteria.Static = true;
            return this;
        }

        IScopeCriteriaBuilder IScopeCriteriaBuilder.DeclaredOnThisType()
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            return this;
        }

        IScopeCriteriaBuilder IScopeCriteriaBuilder.DeclaredOnBaseTypes()
        {
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
            return this;
        }

        void IScopeCriteriaBuilder.All()
        {
            _memberScopeCriteria.DeclaredOnThisType = true;
            _memberScopeCriteria.DeclaredOnBaseTypes = true;
        }

        void IScopeCriteriaBuilder.Default()
        {
            if (_memberScopeCriteria.Instance
                || _memberScopeCriteria.Static
                || _memberScopeCriteria.DeclaredOnBaseTypes
                || _memberScopeCriteria.DeclaredOnThisType)
            {
                throw new InvalidOperationException("Cannot call Default() after calling any of the other methods");
            }
        }
    }
}
