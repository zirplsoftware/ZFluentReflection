using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberQuery : NamedMemberQueryBase<MemberInfo, IMemberQuery>,
        IMemberQuery
    {
        internal MemberQuery(Type type)
            :base(type)
        {
        }

        IMemberTypeSubQuery IMemberQuery.OfMemberType()
        {
            return new MemberTypeSubQuery(this, _memberTypeCriteria);
        }
    }
}
