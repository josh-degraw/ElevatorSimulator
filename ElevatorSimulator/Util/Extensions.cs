using System;
using System.Collections.Generic;
using ElevatorApp.Models;
using ElevatorApp.Models.Enums;

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

        /// <summary>
        /// Extension method to use a <see cref="ElevatorCall"/> as a <see cref="ValueTuple{T1,T2}"/>
        /// </summary>
        /// <example>
        /// <code>
        /// var (dest, direction) = new ElevatorCall(1, Direction.Up);
        /// </code>
        /// </example>
        /// <param name="self">the <see cref="ElevatorCall"/> to deconstruct</param>
        /// <param name="destinationFloor"></param>
        /// <param name="direction"></param>
        public static void Deconstruct(this ElevatorCall self, out int destinationFloor, out Direction direction)
        {
            destinationFloor = self.DestinationFloor;
            direction = self.RequestDirection;
        }
    }
}
