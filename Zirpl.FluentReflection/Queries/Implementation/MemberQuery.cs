using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.CriteriaBuilders;

namespace Zirpl.FluentReflection.Queries.Implementation
{
    internal sealed class MemberQuery : NamedMemberQueryBase<MemberInfo, IMemberQuery>,
        IMemberQuery
    {
        internal MemberQuery(Type type)
            :base(type)
        {
        }

        IMemberQuery IMemberQuery.OfMemberType(Action<IMemberTypeCriteriaBuilder> builder)
        {
            builder(new MemberTypeCriteriaBuilder(MemberTypeFlagsBuilder));
            return this;
        }
    }
}
