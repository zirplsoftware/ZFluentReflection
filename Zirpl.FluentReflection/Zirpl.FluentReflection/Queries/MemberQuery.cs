﻿using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberQuery : NamedTypeMemberQueryBase<MemberInfo, IMemberQuery>,
        IMemberQuery
    {
        internal MemberQuery(Type type)
            :base(type)
        {
        }

        IMemberTypeQuery IMemberQuery.OfMemberType()
        {
            return new MemberTypeSubQuery(this, _memberTypeEvaluator);
        }
    }
}
