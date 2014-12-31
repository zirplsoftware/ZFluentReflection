using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal abstract class CacheableQueryBase<TMemberInfo> : IQueryResult<TMemberInfo>,
        ICacheableQuery<TMemberInfo>
        where TMemberInfo : MemberInfo 
    {
        private String _fromCacheKey;
        private bool _executed;

        protected abstract String CacheKeyPrefix { get; }

        protected abstract IEnumerable<TMemberInfo> ExecuteQuery();

        #region IQueryResult implementation
        IEnumerable<TMemberInfo> IQueryResult<TMemberInfo>.Result()
        {
            if (_executed) throw new InvalidOperationException("Cannot execute twice. Use a new query.");

            _executed = true;
            if (!String.IsNullOrEmpty(_fromCacheKey))
            {
                return (IEnumerable<TMemberInfo>) new CacheService().Get(CacheKeyPrefix + "|" + _fromCacheKey) ??
                       new TMemberInfo[0];
            }
            else
            {
                return ExecuteQuery();
            }
        }

        TMemberInfo IQueryResult<TMemberInfo>.ResultSingle()
        {
            if (_executed) throw new InvalidOperationException("Cannot execute twice. Use a new query.");

            var result = ((IQueryResult<TMemberInfo>)this).Result().ToList();
            if (result.Count() > 1)
                throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result[0];
        }

        TMemberInfo IQueryResult<TMemberInfo>.ResultSingleOrDefault()
        {
            var result = ((IQueryResult<TMemberInfo>)this).Result().ToList();
            if (result.Count() > 1)
                throw new AmbiguousMatchException("Found more than 1 member matching the criteria");

            return result.SingleOrDefault();
        }
        #endregion

        
        void ICacheableQuery<TMemberInfo>.CacheResultTo(string cacheKey)
        {
            if (String.IsNullOrEmpty(cacheKey)) throw new ArgumentNullException("cacheKey");
            new CacheService().Set(CacheKeyPrefix + "|" + cacheKey, ((IQueryResult<TMemberInfo>)this).Result());
        }

        IQueryResult<TMemberInfo> ICacheableQuery<TMemberInfo>.FromCache(string cacheKey)
        {
            if (String.IsNullOrEmpty(cacheKey)) throw new ArgumentNullException("cacheKey");
            _fromCacheKey = cacheKey;
            return this;
        }
    }
}
