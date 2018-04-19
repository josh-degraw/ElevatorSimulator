using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Statistics of <see cref="long"/>, and are aggregated to <see cref="double"/> s.
    /// </summary>
    /// <inheritdoc/>
    public class LongStatistic : Statistic<long, double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LongStatistic"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <inheritdoc/>
        public LongStatistic(string name) : base(name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongStatistic"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        /// <inheritdoc/>
        public LongStatistic(string name, IEnumerable<long> collection) : base(name, collection)
        { }

        protected override long DefaultMin => long.MaxValue;

        /// <summary>
        /// Provides a function to add two items of type <see cref="long"/> together
        /// <para>Represents <c><paramref name="left"/> + <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="left" /> + <paramref name="right" />
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override long AddItems(long left, long right) => left + right;

        /// <summary>
        /// Provides a function to divide one items of type <see cref="long"/> from another
        /// <para>Represents <c><paramref name="numerator"/> / <paramref name="denominator"/></c></para>
        /// </summary>
        /// <param name="numerator">The item on the left of the equation</param>
        /// <param name="denominator">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="numerator" /> / <paramref name="denominator" />
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override long DivideItems(long numerator, long denominator) => numerator / denominator;

        /// <summary>
        /// Provides a function to multiply two items of type <see cref="long"/> together
        /// <para>Represents <c><paramref name="left"/> * <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="left" /> * <paramref name="right" />
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override long MultiplyItems(long left, long right) => left * right;

        /// <summary>
        /// Provides a function to subtract one items of type <see cref="long"/> from another
        /// <para>Represents <c><paramref name="left"/> - <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="left" /> - <paramref name="right" />
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override long SubtractItems(long left, long right) => left - right;

        /// <summary>
        /// A function that will get the average of all statistics held by this object
        /// </summary>
        /// <returns>
        /// The Average value
        /// </returns>
        /// <inheritdoc />
        protected override double CalculateAverage() => this._collection.Average();
    }
}