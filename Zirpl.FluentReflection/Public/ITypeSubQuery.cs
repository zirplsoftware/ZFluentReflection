using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface ITypeSubQuery<out TResult, out TReturnQuery> : IQueryResult<TResult>
        where TResult : MemberInfo
    {
        ITypeSubQuery<TResult, TReturnQuery> AssignableFrom(Type type);
        ITypeSubQuery<TResult, TReturnQuery> AssignableFrom<T>();
        ITypeSubQuery<TResult, TReturnQuery> AssignableFromAll(IEnumerable<Type> types);
        ITypeSubQuery<TResult, TReturnQuery> AssignableFromAny(IEnumerable<Type> types);
        ITypeSubQuery<TResult, TReturnQuery> AssignableTo(Type type);
        ITypeSubQuery<TResult, TReturnQuery> AssignableTo<T>();
        ITypeSubQuery<TResult, TReturnQuery> AssignableToAll(IEnumerable<Type> types);
        ITypeSubQuery<TResult, TReturnQuery> AssignableToAny(IEnumerable<Type> types);
        INameSubQuery<TResult, TReturnQuery> Named();
        INameSubQuery<TResult, TReturnQuery> FullNamed();
        TReturnQuery And();
    }
}