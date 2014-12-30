using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class MemberTypeSubQuery : SubQueryBase<MemberInfo, IMemberQuery>,
        IMemberTypeQuery
    {
        private readonly MemberTypeCriteria _memberTypeCriteria;

        internal MemberTypeSubQuery(IMemberQuery returnQuery, MemberTypeCriteria memberTypeCriteria)
            :base(returnQuery)
        {
            _memberTypeCriteria = memberTypeCriteria;
        }

        IMemberTypeQuery IMemberTypeQuery.Constructor()
        {
            _memberTypeCriteria.Constructor = true;
            return this;
        }

        IMemberTypeQuery IMemberTypeQuery.Event()
        {
            _memberTypeCriteria.Event = true;
            return this;
        }

        IMemberTypeQuery IMemberTypeQuery.Field()
        {
            _memberTypeCriteria.Field = true;
            return this;
        }

        IMemberTypeQuery IMemberTypeQuery.Method()
        {
            _memberTypeCriteria.Method = true;
            return this;
        }

        IMemberTypeQuery IMemberTypeQuery.NestedType()
        {
            _memberTypeCriteria.NestedType = true;
            return this;
        }

        IMemberTypeQuery IMemberTypeQuery.Property()
        {
            _memberTypeCriteria.Property = true;
            return this;
        }

        IMemberQuery IMemberTypeQuery.All()
        {
            _memberTypeCriteria.Constructor = true;
            _memberTypeCriteria.Event = true;
            _memberTypeCriteria.Field = true;
            _memberTypeCriteria.Method = true;
            _memberTypeCriteria.NestedType = true;
            _memberTypeCriteria.Property = true;
            return _returnQuery;
        }

        IMemberQuery IMemberTypeQuery.And()
        {
            return _returnQuery;
        }
    }
}
