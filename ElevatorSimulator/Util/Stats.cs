using NodaTime;
using System;
using System.Collections.Generic;

namespace ElevatorApp.Util
{
    /// <summary>
    /// The Stats class. This class will be able to hold and calculate information about the test during runtime. EG
    /// Average wait...
    /// </summary>
    public class Stats
    {
        /// <summary>
        /// The passenger wait times
        /// </summary>
        [Obsolete]
        private readonly ICollection<Duration> passengerWaitTimes = new AsyncObservableCollection<Duration>();

        /// <summary>
        /// Prevents a default instance of the <see cref="Stats"/> class from being created.
        /// </summary>
        private Stats()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Stats Instance { get; } = new Stats();

        /// <summary>
        /// A representation of the wait times of the passenges in the application
        /// </summary>
        /// <value>The passenger wait times.</value>
        public DurationStatistic PassengerWaitTimes { get; } = new DurationStatistic("Passenger wait times");

        /// <summary>
        /// This function takes a Duration value and adds it to the collection of passenger wait times. It should be used
        /// whenever a passenger exits an elevator
        /// </summary>
        /// <param name="waitTime"></param>
        [Obsolete]
        public void AddPassengerTime(Duration waitTime)
        {
            this.PassengerWaitTimes.Add(waitTime);
        }

        /// <summary>
        /// This function returns a Duration representing the average wait time for passengers in the system.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public Duration GetAverageWaitTime()
        {
            var avg = new Duration();
            if (this.passengerWaitTimes.Count > 0)
            {
                //Aggregate the durations and then return an average
                foreach (Duration d in this.passengerWaitTimes)
                {
                    avg += d;
                }
                avg /= this.passengerWaitTimes.Count;
                return avg;
            }
            else
                return avg;
        }

        /// <summary>
        /// Gets the maximum wait time.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public Duration GetMaxWaitTime()
        {
            Duration max = new Duration();

            if (this.passengerWaitTimes.Count > 0)
            {
                //Return the max value
                foreach (Duration d in this.passengerWaitTimes)
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }

                return max;
            }
            else
                return max;
        }

        /// <summary>
        /// Gets the minimum wait time.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public Duration GetMinWaitTime()
        {
            Duration min = new Duration();

            // for each min value check after the initial one
            if (this.passengerWaitTimes.Count > 0)
            {
                // Temp way to get min a value so it can determine what is smaller.
                min = this.GetMaxWaitTime();

                //Aggregate the durations and then return the min value
                foreach (Duration d in this.passengerWaitTimes)
                {
                    if (d < min)
                    {
                        min = d;
                    }
                }

                return min;
            }
            else
                return min;
        }

        /// <summary>
        /// This returns the number of passengers that have passed through the system and completed their trip
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public int GetPassengerCount()
        {
            return this.passengerWaitTimes.Count;
        }
    }
}