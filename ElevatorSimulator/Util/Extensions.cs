using System;
using System.Collections.Generic;
using MoreLinq;
using System.Linq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Class to Holds helper functions, i.e. Extension methods
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Deconstructs the specified key value pair
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="pair">The pair.</param>
        /// <param name="key">The key.</param>
        /// <param name="val">The value.</param>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue val)
        {
            key = pair.Key;
            val = pair.Value;
        }

        //public static void EnqueueInOrder(this AsyncObservableCollection<int> collection, int value)
        //{
        //    collection.Insert(value - 1, value);
        //}

        /// <summary>
        /// Gets the minimum of all given values, or the <see langword="default"/> if there are no elements or the source is empty
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static TElement MinByOrDefault<TElement, TKey>(this IEnumerable<TElement> collection, Func<TElement, TKey> selector)
        {
            if (collection == null)
                return default;

            try
            {
                return collection.MinBy(selector);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Returns true if any of the given values match the current item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static bool EqualsAny<T>(this T item, params T[] toCompare)
        {
            return toCompare.Any(t => t.Equals(item));
        }

        /// <summary>
        /// Trims a value to be within the given range
        /// </summary>
        /// <param name="me"></param>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        /// <returns></returns>
        public static int KeepInRange(this int me, int min, int max)
        {
            if (me < min)
                return min;

            if (me > max)
                return max;

            return me;


        }

    }

}
