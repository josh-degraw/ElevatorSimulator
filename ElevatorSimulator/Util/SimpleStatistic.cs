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
        /// <param name="name"></param>
        /// <inheritdoc/>
        protected SimpleStatistic(string name) : base(name)
        { }

        /// <inheritdoc/>
        /// <summary>
        /// Constructs a new <see cref="SimpleStatistic{T}"/> object, with the given collection as the initial values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        protected SimpleStatistic(string name, IEnumerable<T> collection) : base(name, collection)
        {
        }
    }
}