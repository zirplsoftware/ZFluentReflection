using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
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
