using System;
using System.Collections.Generic;
using System.Linq;

namespace SBS.IT.Utilities.Shared.UtilityExtension
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable) { action(item); }
        }
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            List<T> nextbatch = new List<T>(batchSize);
            foreach (T item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>(batchSize);
                }
            }
            if (nextbatch.Count > 0)
                yield return nextbatch;
        }
        public static bool IsInSet<T>(this T item, params T[] items)
        {
            if (items == null)
            {
                return false;
            }
            return items.Contains(item);
        }
        public static bool IsInSet<T>(this T item, IEnumerable<T> items)
        {
            if (items == null)
            {
                return false;
            }
            return items.Contains(item);
        }
        public static bool IsBijectiveTo<T>(this IEnumerable<T> lhs, Func<T, T, bool> predicate, IEnumerable<T> rhs)
        {

            var cnt = new Dictionary<T, int>();
            int lhsCnt = 0;
            int rhsCnt = 0;
            Func<T, T> getKeyEvaluator = null;
            bool keyFound = false;

            if (predicate != null)
            {
                getKeyEvaluator = (val) => {
                    var results = cnt.Where((x) => predicate(x.Key, val)).Select(x => x.Key);
                    keyFound = results.Count() > 0;
                    return results.FirstOrDefault();
                };
            }
            else
            {
                getKeyEvaluator = (val) =>
                {
                    keyFound = cnt.ContainsKey(val);
                    return val;
                };
            }

            foreach (T s in lhs)
            {
                if (s != null)
                {
                    var key = getKeyEvaluator(s);
                    if (keyFound) // cnt.ContainsKey(s))
                    {
                        cnt[key]++;
                    }
                    else
                    {
                        cnt.Add(s, 1);
                    }
                }
                lhsCnt++;
            }
            foreach (T s in rhs)
            {
                if (s != null)
                {
                    var key = getKeyEvaluator(s);
                    if (keyFound) //cnt.ContainsKey(s))
                    {
                        cnt[key]--;
                    }
                    else
                    {
                        return false;
                    }
                }
                rhsCnt++;
            }
            return lhsCnt == rhsCnt && cnt.Values.All(c => c == 0);
        }
        public static bool IsBijectiveTo(this IEnumerable<string> lhs, StringComparison stringComparison, IEnumerable<string> rhs)
        {
            return IsBijectiveTo(lhs, (x, y) => string.Equals(x, y, stringComparison), rhs);
        }
        public static bool IsBijectiveTo(this IEnumerable<string> lhs, StringComparison stringComparison, params string[] rhs)
        {
            return IsBijectiveTo(lhs, (x, y) => string.Equals(x, y, stringComparison), rhs.ToList());
        }
        public static bool IsBijectiveTo<T>(this IEnumerable<T> lhs, params T[] rhs)
        {
            return IsBijectiveTo(lhs, null, rhs.ToList());
        }
    }
}
