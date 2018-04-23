using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// The Stats class. This class will be able to hold and calculate information about the test during runtime. EG
    /// Average wait...
    /// </summary>
    public class Stats : IStats, IEnumerable<IStatistic>
    {
        /// <summary>
        /// Gets all of the statistics.
        /// </summary>
        /// <value>
        /// All of the statistics currently tracked
        /// </value>
        /// <inheritdoc/>
        public IReadOnlyList<IStatistic> AllStatistics { get; } = new AsyncObservableCollection<IStatistic>
        {
            new DurationStatistic("Passenger wait times"),
            new DurationStatistic("Passenger ride times"),
            new DurationStatistic("Passenger total times")
        };


        /// <summary>
        /// A representation of the wait times of the passenges in the application
        /// </summary>
        /// <value>The passenger wait times.</value>
        public DurationStatistic PassengerWaitTimes => AllStatistics[0] as DurationStatistic;

        /// <summary>
        /// Gets the passenger ride times.
        /// </summary>
        /// <value>
        /// The total time spent in the elevator
        /// </value>
        public DurationStatistic PassengerRideTimes => AllStatistics[1] as DurationStatistic;

        /// <summary>
        /// Gets the passenger total times.
        /// </summary>
        public DurationStatistic PassengerTotalTimes => AllStatistics[2] as DurationStatistic;

        /// <summary>
        /// Gets the statistics.
        /// </summary>
        /// <value>
        /// The statistics.
        /// </value>
        public static IReadOnlyList<IStatistic> Statistics => Instance.AllStatistics;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Stats Instance { get; } = new Stats();

        /// <summary>
        /// Prevents a default instance of the <see cref="Stats"/> class from being created.
        /// </summary>
        private Stats()
        {
        }

        /// <summary>
        /// This function takes a Duration value and adds it to the collection of passenger wait times. It should be used
        /// whenever a passenger exits an elevator
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="rideTime"></param>
        /// <param name="totalTime"></param>
        [Obsolete]
        public void AddPassengerTime(Duration waitTime, Duration rideTime, Duration totalTime)     // passes this in and knows which to use
        {
            PassengerWaitTimes.Add(waitTime);

            PassengerRideTimes.Add(rideTime);

            PassengerTotalTimes.Add(totalTime);
        }

        /// <inheritdoc />
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<IStatistic> IEnumerable<IStatistic>.GetEnumerator()
        {
            return this.AllStatistics.GetEnumerator();
        }

        /// <summary>
        /// Returns the concatenation of all of the statistics currently tracked
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return AllStatistics.ToDelimitedString("\n\n");
        }

        /// <inheritdoc />
        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this.AllStatistics).GetEnumerator();
        }
    }
}