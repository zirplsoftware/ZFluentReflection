using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class MemberTypeSubQuery : SubQueryBase<MemberInfo, IMemberQuery>,
        IMemberTypeSubQuery
    {
        private readonly MemberTypeCriteria _memberTypeCriteria;

        internal MemberTypeSubQuery(IMemberQuery returnQuery, MemberTypeCriteria memberTypeCriteria)
            :base(returnQuery)
        {
            _memberTypeCriteria = memberTypeCriteria;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Constructor()
        {
            _memberTypeCriteria.Constructor = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Event()
        {
            _memberTypeCriteria.Event = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Field()
        {
            _memberTypeCriteria.Field = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Method()
        {
            _memberTypeCriteria.Method = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.NestedType()
        {
            _memberTypeCriteria.NestedType = true;
            return this;
        }

        IMemberTypeSubQuery IMemberTypeSubQuery.Property()
        {
            _memberTypeCriteria.Property = true;
            return this;
        }

        IMemberQuery IMemberTypeSubQuery.All()
        {
            _memberTypeCriteria.Constructor = true;
            _memberTypeCriteria.Event = true;
            _memberTypeCriteria.Field = true;
            _memberTypeCriteria.Method = true;
            _memberTypeCriteria.NestedType = true;
            _memberTypeCriteria.Property = true;
            return _returnQuery;
        }

        IMemberQuery IMemberTypeSubQuery.And()
        {
            return _returnQuery;
        }
    }
}
