using System;
using System.Collections.Generic;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Implementation.Criteria;

namespace Zirpl.FluentReflection.Queries.Implementation.SubQueries
{
    internal sealed class TypeSubQuery<TResult, TReturnQuery> : SubQueryBase<TResult, TReturnQuery>, 
        ITypeSubQuery<TResult, TReturnQuery>
        where TResult : MemberInfo
        where TReturnQuery : IQueryResult<TResult>
    {
        private readonly TypeCriteria _typeCriteria;

        internal TypeSubQuery(TReturnQuery returnQuery, TypeCriteria typeCriteria)
            : base(returnQuery)
        {
            _typeCriteria = typeCriteria;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFrom(Type type)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = new [] {type};
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFrom<T>()
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = new[] { typeof(T) };
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFromAll(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = types;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFromAny(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableFroms != null) throw new InvalidOperationException("Cannot call more than 1 AssignableFrom-specification method in the same sub-query");

            _typeCriteria.AssignableFroms = types;
            _typeCriteria.AssignableFromAny = true;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableTo(Type type)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = new[] { type };
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableTo<T>()
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = new[] { typeof(T) };
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableToAll(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = types;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableToAny(IEnumerable<Type> types)
        {
            if (_typeCriteria.AssignableTos != null) throw new InvalidOperationException("Cannot call more than 1 AssignableTo-specification method in the same sub-query");

            _typeCriteria.AssignableTos = types;
            _typeCriteria.AssignableToAny = true;
            return this;
        }

        ITypeNameSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.Named()
        {
            return new TypeNameSubQuery<TResult, TReturnQuery>(_returnQuery, _typeCriteria.NameCriteria);
        }

        TReturnQuery ITypeSubQuery<TResult, TReturnQuery>.And()
        {
            return _returnQuery;
        }
    }
}
