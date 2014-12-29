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

        IEnumerable<TResult> IQueryResult<TResult>.Execute()
        {
            return _returnQuery.Execute();
        }

        TResult IQueryResult<TResult>.ExecuteSingle()
        {
            return _returnQuery.ExecuteSingle();
        }

        TResult IQueryResult<TResult>.ExecuteSingleOrDefault()
        {
            return _returnQuery.ExecuteSingleOrDefault();
        }
    }
}
