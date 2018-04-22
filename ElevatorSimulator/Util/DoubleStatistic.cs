using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    ///<inheritdoc/>
    public class DoubleStatistic : SimpleStatistic<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleStatistic"/> class.
        /// </summary>
        /// <param name="name">The name of the statistic.</param>
        /// <inheritdoc/>
        public DoubleStatistic(string name) : base(name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleStatistic"/> class.
        /// </summary>
        /// <param name="name">The name of the statistic.</param>
        /// <param name="collection">A pre-existing collection of values with which to start off the statistic.</param>
        /// <inheritdoc/>
        public DoubleStatistic(string name, IEnumerable<double> collection) : base(name, collection)
        { }

        /// <inheritdoc/>
        /// <summary>
        /// Inherited classes must provide a default minimum value for the type of statistic.
        /// <para>
        /// In most cases, this should be defaulted to a static field or property on the struct or class, titled MaxValue.
        /// </para>
        /// <para>
        /// Defaulting the initial minimum to the maximum possible value ensures that any value that comes after will be smaller
        /// </para>
        /// </summary>
        protected override double DefaultMin => double.MaxValue;

        /// <summary>
        /// Provides a function to add two items of type <see langword="double"/> together
        /// <para>Represents <c><paramref name="left"/> + <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left + right
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override double AddItems(double left, double right) => left + right;

        /// <summary>
        /// Provides a function to divide one item of type <see langword="double"/> from another
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
        protected override double DivideItems(double numerator, double denominator) => numerator / denominator;

        /// <summary>
        /// Provides a function to multiply two items of type <see langword="double"/> together
        /// <para>Represents <c><paramref name="left"/> * <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left * right
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override double MultiplyItems(double left, double right) => left * right;

        /// <summary>
        /// Provides a function to subtract one item of type <see langword="double"/> from another
        /// <para>Represents <c><paramref name="left"/> - <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left - right
        /// </code>
        /// </returns>
        /// <inheritdoc/>
        protected override double SubtractItems(double left, double right) => left - right;

        /// <summary>
        /// A function that will get the average of all statistics held by this object
        /// </summary>
        /// <returns>The Average value</returns>
        /// <inheritdoc/>
        protected override double CalculateAverage() => this._collection.Average();
    }
}