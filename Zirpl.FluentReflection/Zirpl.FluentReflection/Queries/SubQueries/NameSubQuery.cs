using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class NameSubQuery<TMemberInfo, TReturnQuery> : SubQueryBase<TMemberInfo, TReturnQuery>,
        INameQuery<TMemberInfo, TReturnQuery>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly NameCriteria _nameCriteria;

        internal NameSubQuery(TReturnQuery returnQuery, NameCriteria nameCriteria)
            :base(returnQuery)
        {
            _nameCriteria = nameCriteria;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.Exactly(string name)
        {
            _nameCriteria.Name = name;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.Any(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ExactlyIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.AnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWith(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.StartsWith = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWithAny(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.StartsWith = true;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWithIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.StartsWith = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.StartsWith = true;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.Containing(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.Contains = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ContainingAny(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Contains = true;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ContainingIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.Contains = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ContainingAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Contains = true;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWith(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.EndsWith = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWithAny(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.EndsWith = true;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWithIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.EndsWith = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.EndsWith = true;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }
    }
}
