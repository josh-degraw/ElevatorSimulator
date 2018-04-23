using System.Collections.Generic;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Interface for the <see cref="Stats"/> class
    /// </summary>
    public interface IStats
    {
        /// <summary>
        /// Gets all of the statistics.
        /// </summary>
        /// <value>
        /// All of the statistics currently tracked
        /// </value>
        IReadOnlyList<IStatistic> AllStatistics { get; }
    }
}