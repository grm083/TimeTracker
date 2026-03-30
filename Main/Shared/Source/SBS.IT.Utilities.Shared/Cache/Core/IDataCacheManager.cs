namespace SBS.IT.Utilities.Shared.Cache.Core
{
    public interface IDataCacheManager
    {
        object Get(string key);
        T Get<T>() where T : class, new();
        void Set(string key, object data, int cacheTime = 300);
        void Set<T>(T data, int cacheTime = 300) where T : class, new();
        bool IsSet(string key);
        void Remove(string key);
        void Clear();
    }
}
