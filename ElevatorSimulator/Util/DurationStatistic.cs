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
        /// <param name="name">The name of the statistic.</param>
        /// <inheritdoc />
        public DurationStatistic(string name) : base(name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurationStatistic"/> class.
        /// </summary>
        /// <param name="name">The name of the statistic.</param>
        /// <param name="collection">A pre-existing collection of values with which to start off the statistic.</param>
        /// <inheritdoc />
        public DurationStatistic(string name, IEnumerable<Duration> collection) : base(name, collection)
        { }

        /// <inheritdoc />
        /// <summary>
        /// A delegate function to convert aggregated values to a string. Specifies the format of such values. Defaults
        /// to <see cref="M:System.Object.ToString" />
        /// </summary>
        protected override ToStringMethod<Duration> AggregateToString { get; } = d => d.ToString("mm:ss", null);

        /// <inheritdoc />
        /// <summary>
        /// A delegate function to convert individual statistical values to a string. Specifies the format of such
        /// values. Defaults to <see cref="M:System.Object.ToString" />
        /// </summary>
        protected override ToStringMethod<Duration> StatToString { get; } = d => d.ToString("mm:ss", null);

        /// <inheritdoc />
        /// <summary>
        /// Inherited classes must provide a default minimum value for the type of statistic.
        /// <para>In most cases, this should be defaulted to a static field or property on the struct or class, titled MaxValue.</para><para>Defaulting the initial minimum to the maximum possible value ensures that any value that comes after will be smaller</para>
        /// </summary>
        protected override Duration DefaultMin { get; } = Duration.MaxValue;

        /// <inheritdoc />
        /// <summary>
        /// Provides a function to add two items of type <see cref="T:NodaTime.Duration" /> together
        /// <para>Represents <c>left + right</c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left + right
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
        /// Provides a function to divide one item of type <see cref="Duration"/> from another
        /// <para>Represents <c><paramref name="numerator"/> / <paramref name="denominator"/></c></para>
        /// </summary>
        /// <param name="numerator">The item on the left of the equation</param>
        /// <param name="denominator">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// numerator / denominator
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override Duration DivideItems(Duration numerator, Duration denominator) => Duration.FromNanoseconds(numerator / denominator);

        ///<inheritdoc/>
        /// <summary>
        /// Provides a function to multiply two items of type <see cref="Duration"/>  together
        /// <para>
        /// Represents <c>left * right</c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        ///   left * right</code>
        /// </returns>
        protected override Duration MultiplyItems(Duration left, Duration right) => Duration.FromNanoseconds(left.TotalNanoseconds * right.TotalNanoseconds);

        ///<inheritdoc/>
        /// <summary>
        /// Provides a function to subtract one item of type <see cref="Duration"/>  from another
        /// <para>
        /// Represents <c>left - right</c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        ///   left - right</code>
        /// </returns>
        protected override Duration SubtractItems(Duration left, Duration right) => left - right;
    }
}