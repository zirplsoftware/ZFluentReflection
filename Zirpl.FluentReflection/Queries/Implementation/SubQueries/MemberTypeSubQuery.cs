using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.Helpers;

namespace Zirpl.FluentReflection.Queries.Implementation.SubQueries
{
    internal class MemberTypeSubQuery : SubQueryBase<MemberInfo, IMemberQuery>,
        IMemberTypeSubQuery
    {
        private readonly MemberTypeFlagsBuilder _memberTypesFlagsBuilder;

        internal MemberTypeSubQuery(IMemberQuery returnQuery, MemberTypeFlagsBuilder memberTypesFlagsBuilder)
            :base(returnQuery)
        {
            _memberTypesFlagsBuilder = memberTypesFlagsBuilder;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Constructor()
        {
            _memberTypesFlagsBuilder.Constructor = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Event()
        {
            _memberTypesFlagsBuilder.Event = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Field()
        {
            _memberTypesFlagsBuilder.Field = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Method()
        {
            _memberTypesFlagsBuilder.Method = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.NestedType()
        {
            _memberTypesFlagsBuilder.NestedType = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Property()
        {
            _memberTypesFlagsBuilder.Property = true;
            return this;
        }

        IMemberQuery IMemberTypeSubQuery.All()
        {
            _memberTypesFlagsBuilder.Constructor = true;
            _memberTypesFlagsBuilder.Event = true;
            _memberTypesFlagsBuilder.Field = true;
            _memberTypesFlagsBuilder.Method = true;
            _memberTypesFlagsBuilder.NestedType = true;
            _memberTypesFlagsBuilder.Property = true;
            return _returnQuery;
        }

        IMemberQuery IMemberTypeSubQuery.And()
        {
            return _returnQuery;
        }
    }
}
