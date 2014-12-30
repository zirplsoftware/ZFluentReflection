using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class TypeSubQuery<TResult, TReturnQuery> : SubQueryBase<TResult, TReturnQuery>, 
        ITypeQuery<TResult, TReturnQuery>
        where TResult : MemberInfo
        where TReturnQuery : IQueryResult<TResult>
    {
        private readonly TypeCriteria _typeCriteria;

        internal TypeSubQuery(TReturnQuery returnQuery, TypeCriteria typeCriteria)
            : base(returnQuery)
        {
            _typeCriteria = typeCriteria;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableFrom(Type type)
        {
            _typeCriteria.AssignableFrom = type;
            return this;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableFrom<T>()
        {
            _typeCriteria.AssignableFrom = typeof (T);
            return this;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableFromAll(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableFroms = types;
            return this;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableFromAny(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableFroms = types;
            _typeCriteria.Any = true;
            return this;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableTo(Type type)
        {
            _typeCriteria.AssignableTo = type;
            return this;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableTo<T>()
        {
            _typeCriteria.AssignableTo = typeof(T);
            return this;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableToAll(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableTos = types;
            return this;
        }

        ITypeQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.AssignableToAny(IEnumerable<Type> types)
        {
            _typeCriteria.AssignableTos = types;
            _typeCriteria.Any = true;
            return this;
        }

        INameQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.Named()
        {
            return new NameSubQuery<TResult, TReturnQuery>(_returnQuery, _typeCriteria.NameCriteria);
        }

        INameQuery<TResult, TReturnQuery> ITypeQuery<TResult, TReturnQuery>.FullNamed()
        {
            return new NameSubQuery<TResult, TReturnQuery>(_returnQuery, _typeCriteria.FullNameCriteria);
        }

        TReturnQuery ITypeQuery<TResult, TReturnQuery>.And()
        {
            return _returnQuery;
        }
    }
}
