﻿using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal abstract class NamedTypeMemberQueryBase<TMemberInfo, TMemberQuery> : TypeMemberQueryBase<TMemberInfo, TMemberQuery>,
        INamedMemberQuery<TMemberInfo, TMemberQuery>
        where TMemberInfo : MemberInfo 
        where TMemberQuery : INamedMemberQuery<TMemberInfo, TMemberQuery>
    {
        internal NamedTypeMemberQueryBase(Type type)
            :base(type)
        {
            
        }
        INameQuery<TMemberInfo, INamedMemberQuery<TMemberInfo, TMemberQuery>> INamedMemberQuery<TMemberInfo, TMemberQuery>.Named()
        {
            return new NameSubQuery<TMemberInfo, INamedMemberQuery<TMemberInfo, TMemberQuery>>((TMemberQuery)(Object)this, _memberNameEvaluator);
        }
    }
}