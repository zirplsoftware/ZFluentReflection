using System.Collections.Generic;

namespace Zirpl.FluentReflection
{
    internal abstract class SubQueryBase<TResult, TReturnQuery> : IQueryResult<TResult>
        where TReturnQuery : IQueryResult<TResult>
    {
        protected readonly TReturnQuery _returnQuery;

        internal SubQueryBase(TReturnQuery returnQuery)
        {
            _returnQuery = returnQuery;
        }

        IEnumerable<TResult> IQueryResult<TResult>.Result()
        {
            return _returnQuery.Result();
        }

        TResult IQueryResult<TResult>.ResultSingle()
        {
            return _returnQuery.ResultSingle();
        }

        TResult IQueryResult<TResult>.ResultSingleOrDefault()
        {
            return _returnQuery.ResultSingleOrDefault();
        }
    }
}
