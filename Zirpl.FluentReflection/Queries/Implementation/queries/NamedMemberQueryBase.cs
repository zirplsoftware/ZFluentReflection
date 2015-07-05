using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal abstract class NamedMemberQueryBase<TMemberInfo, TMemberQuery> : MemberQueryBase<TMemberInfo, TMemberQuery>,
        INamedMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo 
        where TMemberQuery : INamedMemberQuery<TMemberInfo, TMemberQuery>
    {
        internal NamedMemberQueryBase(Type type)
            : base(type)
        {
        }

        TMemberQuery INamedMemberQuery<TMemberInfo, TMemberQuery>.Named(Action<INameCriteriaBuilder> builder)
        {
            builder(new NameCriteriaBuilder(MemberNameCriteria));
            return (TMemberQuery) (Object) this;
        }
    }
}
