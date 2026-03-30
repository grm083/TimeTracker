using SBS.IT.Utilities.Shared.Cache.Core;
using System.Web;

namespace SBS.IT.Utilities.Shared.Cache.Implementation
{
    public class SessionCacheManager : ISessionCacheManager
    {
        public object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }
        public T Get<T>() where T : class, new()
        {
            return HttpContext.Current.Session[typeof(T).FullName] as T;
        }
        public void Set(string key, object data)
        {
            HttpContext.Current.Session[key] = data;
            HttpContext.Current.Session.Timeout = 60;
        }
        public void Set<T>(T data) where T : class, new()
        {
            HttpContext.Current.Session[typeof(T).FullName] = data;
        }
        public bool IsSet(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return true;
            }
            return false;
        }
        public void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
