﻿using ElevatorApp.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Get a string from a given item.
    /// </summary>
    /// <typeparam name="T">The type of the item</typeparam>
    /// <param name="item">The item.</param>
    /// <returns>A string representation of the given item</returns>
    public delegate string ToStringMethod<in T>(T item);

    /// <inheritdoc />
    /// <summary>
    /// Represents a statistical computation. As new items are added, the new statistical values are calculated and
    /// updated, so that read operations yield no real cost
    /// </summary>
    /// <typeparam name="TStat">The type of statistic that is being calculated</typeparam>
    /// <typeparam name="TAggregate">The type that is used for aggregate values e.g. the average</typeparam>
    public abstract class Statistic<TStat, TAggregate> : ModelBase, IStatistic<TStat, TAggregate>
        where TStat : struct, IComparable<TStat>
        where TAggregate : struct, IComparable<TAggregate>
    {
        private readonly object _locker = new object();

        /// <summary>
        /// The collection of statistics, made available in a read-only fashion
        /// </summary>
        protected IReadOnlyCollection<TStat> _collection => this._stats;

        #region Backing fields

        /// <summary>
        /// The actual collection of statistics
        /// </summary>
        private readonly ConcurrentBag<TStat> _stats;

        private TAggregate _average;
        private int _count;
        private TStat _min;
        private TStat _max;

        #endregion Backing fields

        #region Properties and Statistical values

        /// <summary>
        /// Inherited classes must provide a default minimum value for the type of statistic. 
        /// <para>In most cases, this should be defaulted to a static field or property on the struct or class, titled MaxValue.</para>
        /// <para>Defaulting the initial minimum to the maximum possible value ensures that any value that comes after will be smaller</para>
        /// </summary>
        protected abstract TStat DefaultMin { get; }

        /// <inheritdoc />
        /// <summary>
        /// The average of all values
        /// </summary>
        public TAggregate Average
        {
            get => this._average;
            private set => this.SetProperty(ref this._average, value);
        }

        /// <inheritdoc />
        /// <summary>
        /// The number of values encountered
        /// </summary>
        public int Count
        {
            get => this._count;
            private set => this.SetProperty(ref this._count, value);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the maximum of all the collected values
        /// </summary>
        public TStat Max
        {
            get => this._max;
            private set => this.SetProperty(ref this._max, value);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the minimum of all the collected values
        /// </summary>
        public TStat Min
        {
            get => this._min;
            private set => this.SetProperty(ref this._min, value);
        }

        /// <inheritdoc />
        /// <summary>
        /// The name of the statistic
        /// </summary>
        public string Name { get; }

        #endregion Properties and Statistical values

        #region Abstract Calculations

        /// <summary>
        /// Provides a function to add two items of type <typeparamref name="TStat"/> together
        /// <para>Represents <c><paramref name="left"/> + <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left + right
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat AddItems(TStat left, TStat right);

        /// <summary>
        /// A function that will get the average of all statistics held by this object
        /// </summary>
        /// <returns>The Average value</returns>
        [Pure]
        protected abstract TAggregate CalculateAverage();

        /// <summary>
        /// Provides a function to divide one item of type <typeparamref name="TStat"/> from another
        /// <para>Represents <c><paramref name="numerator"/> / <paramref name="denominator"/></c></para>
        /// </summary>
        /// <param name="numerator">The item on the left of the equation</param>
        /// <param name="denominator">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// numerator / denominator
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat DivideItems(TStat numerator, TStat denominator);

        /// <summary>
        /// Provides a function to multiply two items of type <typeparamref name="TStat"/> together
        /// <para>Represents <c><paramref name="left"/> * <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left * right
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat MultiplyItems(TStat left, TStat right);

        /// <summary>
        /// Provides a function to subtract one item of type <typeparamref name="TStat"/> from another
        /// <para>Represents <c><paramref name="left"/> - <paramref name="right"/></c></para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// left - right
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat SubtractItems(TStat left, TStat right);

        #endregion Abstract Calculations

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Statistic{TStat, TAggregate}"/> class.
        /// </summary>
        /// <param name="name">The name of the statistic.</param>
        /// <inheritdoc />
        protected Statistic(string name)
        {
            this.Name = name;
            this._stats = new ConcurrentBag<TStat>();
            this._min = this.DefaultMin;
        }

        /// <inheritdoc />
        /// <summary>
        /// Constructs a new <see cref="Statistic{TStat, TAggregate}"/> object, with the given collection as the initial values
        /// </summary>
        /// <param name="name">The name of the statistic.</param>
        /// <param name="collection">A pre-existing collection of values with which to start off the statistic.</param>
        protected Statistic(string name, IEnumerable<TStat> collection) : this(name)
        {
            this.AddRange(collection);
        }

        #endregion Constructors

        /// <summary>
        /// A delegate function to convert aggregated values to a string.
        ///
        /// Specifies the format of such values.
        ///
        /// Defaults to <see cref="object.ToString"/>
        /// </summary>
        protected virtual ToStringMethod<TAggregate> AggregateToString { get; } = t => t.ToString();

        /// <summary>
        /// A delegate function to convert individual statistical values to a string.
        ///
        /// Specifies the format of such values.
        ///
        /// Defaults to <see cref="object.ToString"/>
        /// </summary>
        protected virtual ToStringMethod<TStat> StatToString { get; } = t => t.ToString();

        /// <inheritdoc />
        /// <summary>
        /// Adds an item to the collection and calculates the new <see cref="P:ElevatorApp.Util.Statistic`2.Min" />, <see cref="P:ElevatorApp.Util.Statistic`2.Max" />, and
        /// <see cref="P:ElevatorApp.Util.Statistic`2.Average" /> values
        /// </summary>
        /// <param name="item">The next statistical value to be added</param>
        public void Add(TStat item)
        {
            // Lock here so that all of the aggregated values will by atomic per added item
            lock (this._locker)
            {
                this._stats.Add(item);
                this.Count = this._stats.Count;

                if (item.CompareTo(this.Max) > 0) // item > Max
                {
                    this.Max = item;
                }

                if (item.CompareTo(this.Min) < 0) // item < Min
                {
                    this.Min = item;
                }

                this.Average = this.CalculateAverage();
            }
        }

        /// <summary>
        /// Adds a range of items to the collection of statistics, updating the values as they are entered
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<TStat> items)
        {
            foreach (TStat item in items)
            {
                this.Add(item);
            }
        }
        
        /// <summary>
        /// Returns a string representation of the values of the statistic
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        [Pure]
        public override string ToString()
        {
            return string.Format(
$@"{this.Name}
    Average = {this.AggregateToString(this.Average)}; 
    Min = {this.StatToString(this.Min)}; 
    Max = {this.StatToString(this.Max)}; 
    Count = {this.Count}");
        }
    }
}