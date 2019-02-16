using System;
using System.Collections.Generic;
using System.Linq;
using Dzaba.Utils;

namespace Dzaba.HomeAccounting.Utils
{
    public static class Extensions
    {
        public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            Require.NotNull(collection, nameof(collection));
            Require.NotNull(predicate, nameof(predicate));

            var elements = collection.Where(predicate).ToArray();
            foreach (var element in elements)
            {
                collection.Remove(element);
            }
        }

        public static IEnumerable<T> ForEachLazy<T>(this IEnumerable<T> collection, Action<T> action)
        {
            Require.NotNull(collection, nameof(collection));
            Require.NotNull(action, nameof(action));

            return collection.Select(x =>
            {
                action(x);
                return x;
            });
        }
    }
}
