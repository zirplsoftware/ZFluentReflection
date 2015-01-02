using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class TypeNameSubQuery<TMemberInfo, TReturnQuery> : NameSubQuery<TMemberInfo, TReturnQuery>,
        ITypeNameSubQuery<TMemberInfo, TReturnQuery>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly TypeNameCriteria _typeNameCriteria;

        internal TypeNameSubQuery(TReturnQuery returnQuery, TypeNameCriteria typeNameCriteria)
            :base(returnQuery, typeNameCriteria)
        {
            _typeNameCriteria = typeNameCriteria;
        }


        INameSubQuery<TMemberInfo, TReturnQuery> ITypeNameSubQuery<TMemberInfo, TReturnQuery>.Full()
        {
            _typeNameCriteria.UseFullName = true;
            return this;
        }
    }
}
