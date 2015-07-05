using System.Collections.Generic;

namespace Zirpl.FluentReflection.Queries
{
    public interface IQueryResult<out TResult>
    {
        IEnumerable<TResult> Result();
        TResult ResultSingle();
        TResult ResultSingleOrDefault();
    }
}
