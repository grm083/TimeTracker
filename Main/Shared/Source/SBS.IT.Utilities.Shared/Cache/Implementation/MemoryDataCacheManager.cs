using SBS.IT.Utilities.Shared.Cache.Core;
using System;
using System.Runtime.Caching;

namespace SBS.IT.Utilities.Shared.Cache.Implementation
{
    public partial class MemoryDataCacheManager : IDataCacheManager
    {
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }
        public object Get(string key)
        {
            return Cache[key];
        }
        public T Get<T>() where T : class, new()
        {
            return Cache[typeof(T).FullName] as T;
        }
        public void Set(string key, object data, int cacheTime = 300)
        {
            if (data == null)
            {
                return;
            }
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }
        public void Set<T>(T data, int cacheTime = 300) where T : class, new()
        {
            if (data == null)
            {
                return;
            }
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(typeof(T).FullName, data), policy);
        }
        public bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }
        public void Remove(string key)
        {
            Cache.Remove(key);
        }
        public void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
    }
}
