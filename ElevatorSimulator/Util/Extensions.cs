using System;
using System.Collections.Generic;

namespace ElevatorApp.Util
{
    public static partial class Extensions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue val)
        {
            key = pair.Key;
            val = pair.Value;
        }

        private delegate bool CompareFunction<in T>(T thisVal, T valToCheckAgainst) where T : IComparable<T>;

        //private static TItem AggregatedLimit<TItem, TComparer>(this ICollection<TItem> collection, Func<TItem, TComparer> comparer, CompareFunction<TComparer> compareFunction) where TComparer : IComparable<TComparer>
        //{
        //    var (maxItem, maxVal) = (default(TItem), default(TComparer));

        //    foreach (TItem item in collection)
        //    {
        //        TComparer next = comparer(item);

        //        if (compareFunction(next, maxVal))
        //        {
        //            (maxItem, maxVal) = (item, next);
        //        }
        //    }

        //    return maxItem;
        //}

        //public static TItem MinBy<TItem, TComparer>(this ICollection<TItem> collection, Func<TItem, TComparer> comparer) where TComparer : IComparable<TComparer>
        //{
        //    return collection.AggregatedLimit(comparer, (a, min) => a.CompareTo(min) < 0);
        //}

        //public static TItem MaxBy<TItem, TComparer>(this ICollection<TItem> collection, Func<TItem, TComparer> comparer) where TComparer : IComparable<TComparer>
        //{
        //    return collection.AggregatedLimit(comparer, (a, min) => a.CompareTo(min) > 0);
        //}

    }
}
