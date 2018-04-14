using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ElevatorApp.Models;

namespace ElevatorApp.Util
{
    public delegate string ToStringMethod<in T>(T item);

    /// <summary>
    /// The base interface for statistical calculations
    /// </summary>
    public interface IStatistic
    {
        /// <summary>
        /// The number of values encountered
        /// </summary>
        int Count { get; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TStat"></typeparam>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IStatistic<TStat, TAggregate> : IStatistic
    {
        string Name { get; }

        TStat Min { get; }
        TStat Max { get; }
        TAggregate Average { get; }
    }

    /// <summary>
    /// Represents a statistical computation. As new items are added, the new statistical values are calculated and updated, so that read operations yield no real cost 
    /// </summary>
    /// <typeparam name="TStat">The type of statistic that is being calculated</typeparam>
    /// <typeparam name="TAggregate">The type that is used for aggregate values e.g. the average</typeparam>
    public abstract class Statistic<TStat, TAggregate> : ModelBase, IStatistic<TStat, TAggregate>
        where TStat : struct, IComparable<TStat>
        where TAggregate : struct, IComparable<TAggregate>
    {
        /// <summary>
        /// The collection of statistics, made available in a read-only fashion
        /// </summary>
        protected IReadOnlyCollection<TStat> _collection => _stats;

        #region Backing fields
        /// <summary>
        /// The actual collection of statistics
        /// </summary>
        private readonly ConcurrentBag<TStat> _stats;

        private int _count;
        private TStat _min, _max;
        private TAggregate _average;


        #endregion

        #region Properties and Statistical values

        public string Name { get; }

        /// <summary>
        /// The minimum value encountered
        /// </summary>
        public TStat Min
        {
            get => _min;
            private set => SetProperty(ref _min, value);
        }

        /// <summary>
        /// The maximum value encountered
        /// </summary>
        public TStat Max
        {
            get => _max;
            private set => SetProperty(ref _max, value);
        }

        /// <summary>
        /// The average of all values
        /// </summary>
        public TAggregate Average
        {
            get => _average;
            private set => SetProperty(ref _average, value);
        }

        /// <summary>
        /// The number of values encountered
        /// </summary>
        public int Count
        {
            get => _count;
            private set => SetProperty(ref _count, value);
        }

        #endregion

        #region Abstract Calculations
        /// <summary>
        /// Provides a function to add two items of type <typeparamref name="TStat"/> together
        /// <para>
        /// Represents <c><paramref name="left"/> + <paramref name="right"/></c>
        /// </para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="left"/> + <paramref name="right"/>
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat AddItems(TStat left, TStat right);

        /// <summary>
        /// Provides a function to subtract one items of type <typeparamref name="TStat"/> from another
        /// <para>
        /// Represents <c><paramref name="left"/> - <paramref name="right"/></c>
        /// </para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="left"/> - <paramref name="right"/>
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat SubtractItems(TStat left, TStat right);


        /// <summary>
        /// Provides a function to multiply two items of type <typeparamref name="TStat"/> together
        /// <para>
        /// Represents <c><paramref name="left"/> * <paramref name="right"/></c>
        /// </para>
        /// </summary>
        /// <param name="left">The item on the left of the equation</param>
        /// <param name="right">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="left"/> * <paramref name="right"/>
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat MultiplyItems(TStat left, TStat right);

        /// <summary>
        /// Provides a function to divide one items of type <typeparamref name="TStat"/> from another
        /// <para>
        /// Represents <c><paramref name="numerator"/> / <paramref name="denominator"/></c>
        /// </para>
        /// </summary>
        /// <param name="numerator">The item on the left of the equation</param>
        /// <param name="denominator">The item on the right of the equation</param>
        /// <returns>
        /// <code>
        /// <paramref name="numerator"/> / <paramref name="denominator"/>
        /// </code>
        /// </returns>
        [Pure]
        protected abstract TStat DivideItems(TStat numerator, TStat denominator);

        /// <summary>
        /// A function that will get the average of all statistics held by this object
        /// </summary>
        /// <returns>The Average value</returns>
        [Pure]
        protected abstract TAggregate CalculateAverage();

        #endregion

        #region Constructors
        ///<inheritdoc/>
        /// <summary>
        /// Constructs a new <see cref="Statistic{TStat, TAggregate}"/> object
        /// </summary>
        protected Statistic(string name)
        {
            this.Name = name;
            _stats = new ConcurrentBag<TStat>();
        }


        /// <summary>
        /// Constructs a new <see cref="Statistic{TStat, TAggregate}"/> object, with the given collection as the initial values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        protected Statistic(string name, IEnumerable<TStat> collection) : this(name)
        {
            this.AddRange(collection);
        }

        #endregion

        /// <summary>
        /// Adds an item to the collection and calculates the new <see cref="Min"/>, <see cref="Max"/>, and <see cref="Average"/> values
        /// </summary>
        /// <param name="item">The next statistical value to be added</param>
        public void Add(TStat item)
        {
            _stats.Add(item);
            Count = _stats.Count;

            if (item.CompareTo(Max) > 0) // item > Max
            {
                Max = item;
            }
            
            if (item.CompareTo(Min) < 0) // item < Min
            {
                Min = item;
            }

            Average = CalculateAverage();
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
        
        protected virtual ToStringMethod<TStat> StateToString { get; } = t => t.ToString();
        protected virtual ToStringMethod<TAggregate> AggregateToString { get; } = t => t.ToString();

        /// <inheritdoc/>
        [Pure]
        public override string ToString()
        {

            return string.Format($@"Average = {AggregateToString(Average)}; Min = {StateToString(Min)}; Max = {StateToString(Max)}; Count = {Count}");
        }
    }

    /// <inheritdoc/>
    /// <summary>
    /// Represents a statistical computation which aggregates to the same type. As new items are added, the new statistical values are calculated and updated, so that read operations yield no real cost 
    /// </summary>
    /// <typeparam name="T">The single calculation type</typeparam>
    public abstract class SimpleStatistic<T> : Statistic<T, T> where T : struct, IComparable<T>
    {
        /// <inheritdoc/>
        protected SimpleStatistic(string name) : base(name)
        { }

        /// <inheritdoc/>
        /// <summary>
        /// Constructs a new <see cref="SimpleStatistic{T}"/> object, with the given collection as the initial values
        /// </summary>
        /// <param name="collection"></param>
        protected SimpleStatistic(string name, IEnumerable<T> collection) : base(name, collection)
        {
        }
    }
}