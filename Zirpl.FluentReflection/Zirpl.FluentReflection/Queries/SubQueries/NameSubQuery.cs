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
        private readonly NameEvaluator _nameEvaluator;

        internal NameSubQuery(TReturnQuery returnQuery, NameEvaluator nameEvaluator)
            :base(returnQuery)
        {
            _nameEvaluator = nameEvaluator;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.Exactly(string name)
        {
            _nameEvaluator.Name = name;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.Any(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ExactlyIgnoreCase(string name)
        {
            _nameEvaluator.Name = name;
            _nameEvaluator.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.AnyIgnoreCase(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.Any = true;
            _nameEvaluator.IgnoreCase = true;
            return (TReturnQuery)(Object)this;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWith(string name)
        {
            _nameEvaluator.Name = name;
            _nameEvaluator.StartsWith = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWithAny(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.StartsWith = true;
            _nameEvaluator.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWithIgnoreCase(string name)
        {
            _nameEvaluator.Name = name;
            _nameEvaluator.StartsWith = true;
            _nameEvaluator.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.StartingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.StartsWith = true;
            _nameEvaluator.Any = true;
            _nameEvaluator.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.Containing(string name)
        {
            _nameEvaluator.Name = name;
            _nameEvaluator.Contains = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ContainingAny(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.Contains = true;
            _nameEvaluator.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ContainingIgnoreCase(string name)
        {
            _nameEvaluator.Name = name;
            _nameEvaluator.Contains = true;
            _nameEvaluator.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.ContainingAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.Contains = true;
            _nameEvaluator.Any = true;
            _nameEvaluator.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWith(string name)
        {
            _nameEvaluator.Name = name;
            _nameEvaluator.EndsWith = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWithAny(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.EndsWith = true;
            _nameEvaluator.Any = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWithIgnoreCase(string name)
        {
            _nameEvaluator.Name = name;
            _nameEvaluator.EndsWith = true;
            _nameEvaluator.IgnoreCase = true;
            return _returnQuery;
        }

        TReturnQuery INameQuery<TMemberInfo, TReturnQuery>.EndingWithAnyIgnoreCase(IEnumerable<string> names)
        {
            _nameEvaluator.Names = names;
            _nameEvaluator.EndsWith = true;
            _nameEvaluator.Any = true;
            _nameEvaluator.IgnoreCase = true;
            return _returnQuery;
        }
    }
}
