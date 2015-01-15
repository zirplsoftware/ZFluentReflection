using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.SubQueries;

namespace Zirpl.FluentReflection.Queries.Implementation
{
    internal abstract class NamedMemberQueryBase<TMemberInfo, TMemberQuery> : MemberQueryBase<TMemberInfo, TMemberQuery>,
        INamedMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo 
        where TMemberQuery : INamedMemberQuery<TMemberInfo, TMemberQuery>
    {
        internal NamedMemberQueryBase(Type type)
            :base(type)
        {
            
        }
        INameSubQuery<TMemberInfo, INamedMemberQuery<TMemberInfo, TMemberQuery>> INamedMemberQuery<TMemberInfo, TMemberQuery>.Named()
        {
            return new NameSubQuery<TMemberInfo, INamedMemberQuery<TMemberInfo, TMemberQuery>>((TMemberQuery)(Object)this, MemberNameCriteria);
        }
    }
}
