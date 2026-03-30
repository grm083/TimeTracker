using System;
using System.Linq;
using System.Linq.Expressions;

namespace SBS.IT.Utilities.Shared.UtilityExtension
{
    public static class QueryableExtension
    {
        public static IQueryable<T> OrderByColumnName<T>(this IQueryable<T> q, string SortColumnName, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortColumnName);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
