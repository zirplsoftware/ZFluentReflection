using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.SubQueries;

namespace Zirpl.FluentReflection.Queries.Implementation
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
