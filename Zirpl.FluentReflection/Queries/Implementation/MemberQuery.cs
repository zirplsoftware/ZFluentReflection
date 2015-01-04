using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.SubQueries;

namespace Zirpl.FluentReflection.Queries
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
            return new MemberTypeSubQuery(this, MemberTypeFlagsBuilder);
        }
    }
}
