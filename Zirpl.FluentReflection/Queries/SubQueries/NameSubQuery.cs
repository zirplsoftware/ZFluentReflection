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
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new [] {name};
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.Any(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ExactlyIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.AnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWith(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWithAny(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWithIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.StartingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.StartsWith;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.Containing(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ContainingAny(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ContainingIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.ContainingAnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.Contains;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWith(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWithAny(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWithIgnoreCase(string name)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = new[] { name };
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameSubQuery<TMemberInfo, TReturnQuery>.EndingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            if (_nameCriteria.Names != null) throw new InvalidOperationException("Cannot call more than 1 Name-specification method in the same sub-query");

            _nameCriteria.Names = names;
            _nameCriteria.NameHandling = NameHandlingType.EndsWith;
            _nameCriteria.IgnoreCase = true;
            return _returnQuery;
        }
    }
}
