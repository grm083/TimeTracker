namespace SBS.IT.Utilities.Shared.Cache.Core
{
    public interface ISessionCacheManager
    {
        object Get(string key);
        T Get<T>() where T : class, new();
        void Set(string key, object data);
        void Set<T>(T data) where T : class, new();
        bool IsSet(string key);
        void Remove(string key);
        void Clear();
    }
}
