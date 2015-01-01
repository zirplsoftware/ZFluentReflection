﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection
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
            _typeCriteria.AssignableFroms = new [] {type};
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFrom<T>()
        {
            _typeCriteria.AssignableFroms = new[] { typeof(T) };
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFromAll(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableFroms = types;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFromAny(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableFroms = types;
            _typeCriteria.Any = true;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableTo(Type type)
        {
            _typeCriteria.AssignableTos = new[] { type };
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableTo<T>()
        {
            _typeCriteria.AssignableTos = new[] { typeof(T) };
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableToAll(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableTos = types;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableToAny(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableTos = types;
            _typeCriteria.Any = true;
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
