using System;
using System.Reflection;

namespace Zirpl.FluentReflection
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
        TMemberQuery INamedMemberQuery<TMemberInfo, TMemberQuery>.Named(String name)
        {
            MemberNameCriteria.Names = new [] {name};
            return (TMemberQuery)(Object)this;
        }
    }
}
