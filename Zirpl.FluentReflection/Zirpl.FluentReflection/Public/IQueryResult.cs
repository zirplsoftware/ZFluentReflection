using System.Collections.Generic;

namespace Zirpl.FluentReflection
{
    public interface IQueryResult<out TResult>
    {
        IEnumerable<TResult> Execute();
        TResult ExecuteSingle();
        TResult ExecuteSingleOrDefault();
    }
}
