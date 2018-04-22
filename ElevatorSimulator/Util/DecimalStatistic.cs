﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Statistical values of type <see cref="decimal"/>
    /// </summary>
    /// <seealso cref="ElevatorApp.Util.SimpleStatistic{T}" />
    /// <inheritdoc />
    public class DecimalStatistic : SimpleStatistic<decimal>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalStatistic"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <inheritdoc />
        public DecimalStatistic(string name) : base(name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalStatistic"/> class.
        /// </summary>
        /// <param name="name">The name of </param>
        /// <param name="collection"></param>
        /// <inheritdoc />
        public DecimalStatistic(string name, IEnumerable<decimal> collection) : base(name, collection)
        { }

        /// <inheritdoc />
        /// <summary>
        /// Inherited classes must provide a default minimum value for the type of statistic.
        /// <para>In most cases, this should be defaulted to a static field or property on the struct or class, titled MaxValue.</para><para>Defaulting the initial minimum to the maximum possible value ensures that any value that comes after will be smaller</para>
        /// </summary>
        protected override decimal DefaultMin { get; } = Decimal.MaxValue;

        /// <summary>
        /// Provides a function to add two items of type <see langword="decimal"/> together
        /// <para>Represents <c><paramref name="left" /> + <paramref name="right" /></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left + right
        /// </code>
        /// </returns>
        /// <inheritdoc />
        protected override decimal AddItems(decimal left, decimal right) => left + right;

        /// <summary>
        /// Provides a function to divide one item of type <see langword="decimal"/> from another
        /// <para>Represents <c><paramref name="numerator" /> / <paramref name="denominator" /></c></para>
        /// </summary>
        /// <param name="numerator">The item on the left of the equation</param>
        /// <param name="denominator">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// numerator / denominator
        /// </code>
        /// </returns>
        /// <inheritdoc />
        protected override decimal DivideItems(decimal numerator, decimal denominator) => numerator / denominator;

        /// <summary>
        /// Provides a function to multiply two items of type <see langword="decimal"/> together
        /// <para>Represents <c><paramref name="left" /> * <paramref name="right" /></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left * right
        /// </code>
        /// </returns>
        /// <inheritdoc />
        protected override decimal MultiplyItems(decimal left, decimal right) => left * right;

        /// <summary>
        /// Provides a function to subtract one item of type <see langword="decimal"/> from another
        /// <para>Represents <c><paramref name="left" /> - <paramref name="right" /></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left - right
        /// </code>
        /// </returns>
        /// <inheritdoc />
        protected override decimal SubtractItems(decimal left, decimal right) => left - right;

        /// <summary>
        /// A function that will get the average of all statistics held by this object
        /// </summary>
        /// <returns>
        /// The Average value
        /// </returns>
        /// <inheritdoc />
        protected override decimal CalculateAverage() => this._collection.Average();
    }
}