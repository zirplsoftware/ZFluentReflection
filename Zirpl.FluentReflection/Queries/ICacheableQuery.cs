using System;

namespace Zirpl.FluentReflection.Queries
{
    public interface ICacheableQuery<out TResult>
    {
        void CacheResultTo(String cacheKey);
        IQueryResult<TResult> FromCache(String cacheKey);
    }
}