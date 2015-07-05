using Zirpl.FluentReflection.Queries.Implementation.Helpers;

namespace Zirpl.FluentReflection.Queries.Implementation.CriteriaBuilders
{
    internal class MemberTypeCriteriaBuilder : IMemberTypeCriteriaBuilder
    {
        private readonly MemberTypeFlagsBuilder _memberTypesFlagsBuilder;

        internal MemberTypeCriteriaBuilder(MemberTypeFlagsBuilder memberTypesFlagsBuilder)
        {
            _memberTypesFlagsBuilder = memberTypesFlagsBuilder;
        }

        IMemberTypeCriteriaBuilder IMemberTypeCriteriaBuilder.Constructor()
        {
            _memberTypesFlagsBuilder.Constructor = true;
            return this;
        }

        IMemberTypeCriteriaBuilder IMemberTypeCriteriaBuilder.Event()
        {
            _memberTypesFlagsBuilder.Event = true;
            return this;
        }

        IMemberTypeCriteriaBuilder IMemberTypeCriteriaBuilder.Field()
        {
            _memberTypesFlagsBuilder.Field = true;
            return this;
        }

        IMemberTypeCriteriaBuilder IMemberTypeCriteriaBuilder.Method()
        {
            _memberTypesFlagsBuilder.Method = true;
            return this;
        }

        IMemberTypeCriteriaBuilder IMemberTypeCriteriaBuilder.NestedType()
        {
            _memberTypesFlagsBuilder.NestedType = true;
            return this;
        }

        IMemberTypeCriteriaBuilder IMemberTypeCriteriaBuilder.Property()
        {
            _memberTypesFlagsBuilder.Property = true;
            return this;
        }

        void IMemberTypeCriteriaBuilder.All()
        {
            _memberTypesFlagsBuilder.Constructor = true;
            _memberTypesFlagsBuilder.Event = true;
            _memberTypesFlagsBuilder.Field = true;
            _memberTypesFlagsBuilder.Method = true;
            _memberTypesFlagsBuilder.NestedType = true;
            _memberTypesFlagsBuilder.Property = true;
        }
    }
}
