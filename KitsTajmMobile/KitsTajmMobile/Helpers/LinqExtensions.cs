using System;
using System.Collections.Generic;

namespace KitsTajmMobile.Helpers
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(
            this IEnumerable<T> source, Func<T, T, bool> predicate)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                    yield break;

                var list = new List<T> { iterator.Current };

                var previous = iterator.Current;

                while (iterator.MoveNext())
                {
                    if (!predicate(previous, iterator.Current))
                    {
                        yield return list;
                        list = new List<T>();
                    }

                    list.Add(iterator.Current);
                    previous = iterator.Current;
                }

                yield return list;
            }
        }
    }
}
