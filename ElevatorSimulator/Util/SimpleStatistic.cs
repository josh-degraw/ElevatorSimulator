using System;
using System.Collections.Generic;

namespace ElevatorApp.Util
{
    /// <inheritdoc/>
    /// <summary>
    /// Represents a statistical computation which aggregates to the same type. As new items are added, the new
    /// statistical values are calculated and updated, so that read operations yield no real cost
    /// </summary>
    /// <typeparam name="T">The single calculation type</typeparam>
    public abstract class SimpleStatistic<T> : Statistic<T, T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStatistic{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the statistic.</param>
        /// <inheritdoc />
        protected SimpleStatistic(string name) : base(name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStatistic{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the statistic.</param>
        /// <param name="collection">A pre-existing collection of values with which to start off the statistic.</param>
        /// <inheritdoc />
        protected SimpleStatistic(string name, IEnumerable<T> collection) : base(name, collection)
        {
        }
    }
}