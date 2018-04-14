using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ElevatorApp.Models;
using ElevatorApp.Models.Enums;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Holds helper functions
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="pair"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue val)
        {
            key = pair.Key;
            val = pair.Value;
        }

        //public static void EnqueueInOrder(this AsyncObservableCollection<int> collection, int value)
        //{
        //    collection.Insert(value - 1, value);
        //}

        public static T MinByOrDefault<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> selector)
        {
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
