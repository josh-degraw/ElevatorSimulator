using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// A statistic comprising values of type <see cref="Duration"/>
    /// </summary>
    /// <inheritdoc/>
    public class DurationStatistic : SimpleStatistic<Duration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DurationStatistic"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <inheritdoc/>
        public DurationStatistic(string name) : base(name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurationStatistic"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        /// <inheritdoc/>
        public DurationStatistic(string name, IEnumerable<Duration> collection) : base(name, collection)
        { }

        /// <summary>
        /// A delegate function to convert aggregated values to a string. Specifies the format of such values. Defaults
        /// to <see cref="M:System.Object.ToString"/>
        /// </summary>
        protected override ToStringMethod<Duration> AggregateToString { get; } = d => d.ToString("mm:ss", null);

        /// <summary>
        /// A delegate function to convert individual statistical values to a string. Specifies the format of such
        /// values. Defaults to <see cref="M:System.Object.ToString"/>
        /// </summary>
        protected override ToStringMethod<Duration> StatToString { get; } = d => d.ToString("mm:ss", null);

        protected override Duration DefaultMin => Duration.MaxValue;

        /// <summary>
        /// Provides a function to add two items of type <see cref="Duration"/> together
        /// <para>Represents <c><paramref name="left"/> + <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="left" /> + <paramref name="right" />
        /// </code>
        /// </returns>
        protected override Duration AddItems(Duration left, Duration right) => left + right;

        /// <summary>
        /// A function that will get the average of all statistics held by this object
        /// </summary>
        /// <returns>The Average value</returns>
        /// <inheritdoc/>
        protected override Duration CalculateAverage() => Duration.FromNanoseconds(this._collection.Average(d => d.TotalNanoseconds));

        /// <summary>
        /// Provides a function to divide one items of type <see cref="Duration"/> from another
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
        protected override Duration DivideItems(Duration numerator, Duration denominator) => Duration.FromNanoseconds(numerator / denominator);

        ///<inheritdoc/>
        /// <summary>
        /// Provides a function to multiply two items of type <see cref="Duration"/>  together
        /// <para>
        /// Represents <c><paramref name="left" /> * <paramref name="right" /></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        ///   <paramref name="left" /> * <paramref name="right" /></code>
        /// </returns>
        protected override Duration MultiplyItems(Duration left, Duration right) => Duration.FromNanoseconds(left.TotalNanoseconds * right.TotalNanoseconds);

        ///<inheritdoc/>
        /// <summary>
        /// Provides a function to subtract one items of type <see cref="Duration"/>  from another
        /// <para>
        /// Represents <c><paramref name="left" /> - <paramref name="right" /></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        ///   <paramref name="left" /> - <paramref name="right" /></code>
        /// </returns>
        protected override Duration SubtractItems(Duration left, Duration right) => left - right;
    }
}