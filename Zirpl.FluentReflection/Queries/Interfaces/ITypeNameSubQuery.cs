using System;
using System.Collections.Generic;

namespace Zirpl.FluentReflection
{
    public interface ITypeNameSubQuery<out TResult, out TReturnQuery> : INameSubQuery<TResult, TReturnQuery>
    {
        INameSubQuery<TResult, TReturnQuery> Full();
    }
}
