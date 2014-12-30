using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal class NameSubQuery<TMemberInfo, TReturnQuery> : SubQueryBase<TMemberInfo, TReturnQuery>,
        INameSubQuery<TMemberInfo, TReturnQuery>
        where TMemberInfo : MemberInfo
        where TReturnQuery : IQueryResult<TMemberInfo>
    {
        private readonly NameCriteria _nameCriteria;

        internal NameSubQuery(TReturnQuery returnQuery, NameCriteria nameCriteria)
            :base(returnQuery)
        {
            _nameCriteria = nameCriteria;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.Exactly(string name)
        {
            _nameCriteria.Name = name;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.Any(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ExactlyIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.AnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWith(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.StartsWith = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWithAny(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.StartsWith = true;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWithIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.StartsWith = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.StartsWith = true;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.Containing(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.Contains = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ContainingAny(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Contains = true;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ContainingIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.Contains = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ContainingAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.Contains = true;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWith(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.EndsWith = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWithAny(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.EndsWith = true;
            _nameCriteria.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWithIgnoreCase(string name)
        {
            _nameCriteria.Name = name;
            _nameCriteria.EndsWith = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameCriteria.Names = names;
            _nameCriteria.EndsWith = true;
            _nameCriteria.Any = true;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }
    }
}
