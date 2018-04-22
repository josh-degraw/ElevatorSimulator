namespace ElevatorApp.Util
{
    /// <summary>
    /// The base interface for statistical calculations
    /// </summary>
    public interface IStatistic
    {
        /// <summary>
        /// The number of values encountered
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The name of the statistic
        /// </summary>
        string Name { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// An interface representing a statistical calculation of the given types
    /// </summary>
    /// <typeparam name="TStat">The type of the stat.</typeparam>
    /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
    public interface IStatistic<TStat, out TAggregate> : IStatistic
    {
        /// <summary>
        /// Gets the average of all the collected values
        /// </summary>
        TAggregate Average { get; }

        /// <summary>
        /// Gets the maximum of all the collected values
        /// </summary>
        TStat Max { get; }

        /// <summary>
        /// Gets the minimum of all the collected values
        /// </summary>
        TStat Min { get; }

        /// <summary>
        /// Adds the specified item to the collection of statistics
        /// </summary>
        /// <param name="item">The item.</param>
        void Add(TStat item);
    }
}