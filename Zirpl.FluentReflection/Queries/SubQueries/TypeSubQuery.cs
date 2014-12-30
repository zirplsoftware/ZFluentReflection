using System;
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
            _typeCriteria.AssignableFrom = type;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableFrom<T>()
        {
            _typeCriteria.AssignableFrom = typeof (T);
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
            _typeCriteria.AssignableTo = type;
            return this;
        }

        ITypeSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.AssignableTo<T>()
        {
            _typeCriteria.AssignableTo = typeof(T);
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

        INameSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.Named()
        {
            return new NameSubQuery<TResult, TReturnQuery>(_returnQuery, _typeCriteria.NameCriteria);
        }

        INameSubQuery<TResult, TReturnQuery> ITypeSubQuery<TResult, TReturnQuery>.FullNamed()
        {
            return new NameSubQuery<TResult, TReturnQuery>(_returnQuery, _typeCriteria.FullNameCriteria);
        }

        TReturnQuery ITypeSubQuery<TResult, TReturnQuery>.And()
        {
            return _returnQuery;
        }
    }
}
