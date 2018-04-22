using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// The Stats class. This class will be able to hold and calculate information about the test during runtime. EG
    /// Average wait...
    /// </summary>
    public class Stats
    {
        /// <summary>
        /// A representation of the wait times of the passenges in the application
        /// </summary>
        /// <value>The passenger wait times.</value>
        public DurationStatistic PassengerWaitTimes { get; } = new DurationStatistic("Passenger wait times");

        /// <summary>
        /// Gets the passenger ride times.
        /// </summary>
        public DurationStatistic PassengerRideTimes { get; } = new DurationStatistic("Passenger ride times");

        /// <summary>
        /// Gets the passenger total times.
        /// </summary>
        public DurationStatistic PassengerTotalTimes { get; } = new DurationStatistic("Passenger total times");
        
        private IEnumerable<IStatistic> _getStatistics()
        {
            yield return this.PassengerWaitTimes;
            yield return this.PassengerRideTimes;
            yield return this.PassengerTotalTimes;
        }

        /// <summary>
        /// Gets all statistics tracked by the <see cref="Stats"/> class
        /// </summary>
        public IEnumerable<IStatistic> AllStatistics => _getStatistics();
        

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

        public void AddPassengerTime(Duration waitTime, Duration rideTime, Duration totalTime)     // passes this in and knows which to use
        {
            PassengerWaitTimes.Add(waitTime);

            PassengerWaitTimes.Add(rideTime);

            PassengerWaitTimes.Add(totalTime);
        }

        // i need to potentially change this to take a duration and an int to determine which stat to add to if int == 1
        // do the wait, it == 2 do the ride, etc make sure these new functions are the right visibility

        // make the functions here usable by all types by passing in the kind you want

        /// <summary>
        /// This function returns a Duration representing the average wait time for passengers in the system.
        /// </summary>
        /// <returns></returns>
        //public Duration GetAverageTime(int pass)
        //{
        //    Duration avg = new Duration();
        //    if (pass == 1)
        //    {
        //        if (passengerWaitTimes.Count > 0)
        //        {
        //            //Aggregate the durations and then return an average
        //            foreach (Duration d in passengerWaitTimes)
        //            {
        //                avg += d;
        //            }
        //            avg /= passengerWaitTimes.Count;
        //            return avg;
        //        }
        //        else
        //            return avg;
        //    }
        //    else if (pass == 2)
        //    {
        //        if (passengerRideTimes.Count > 0)
        //        {
        //            //Aggregate the durations and then return an average
        //            foreach (Duration d in passengerRideTimes)
        //            {
        //                avg += d;
        //            }
        //            avg /= passengerRideTimes.Count;
        //            return avg;
        //        }
        //        else
        //            return avg;
        //    }
        //    else if (pass == 3)
        //    {
        //        if (passengerTotalTimes.Count > 0)
        //        {
        //            //Aggregate the durations and then return an average
        //            foreach (Duration d in passengerTotalTimes)
        //            {
        //                avg += d;
        //            }
        //            avg /= passengerTotalTimes.Count;
        //            return avg;
        //        }
        //        else
        //            return avg;
        //    }
        //    else
        //        return avg;
        //}

        //public Duration GetMinTime(int pass)
        //{
        //    Duration min = new Duration();

        //    // for each min value check after the initial one

        //    if (pass == 1)
        //    {
        //        tempTimes = passengerWaitTimes;
        //    }
        //    else if (pass == 2)
        //    {
        //        tempTimes = passengerRideTimes;
        //    }
        //    else if (pass == 3)
        //    {
        //        tempTimes = passengerTotalTimes;
        //    }

        //    if (tempTimes.Count > 0)
        //    {
        //        // Temp way to get min a value so it can determine what is smaller.
        //        min = GetMaxTime(pass);

        //        //Aggregate the durations and then return the min value
        //        foreach (Duration d in tempTimes)
        //        {
        //            if (d < min)
        //            {
        //                min = d;
        //            }
        //        }

        //        return min;
        //    }
        //    else
        //        return min;
        //}

        //public Duration GetMaxTime(int pass)
        //{
        //    Duration max = new Duration();

        //    // for each min value check after the initial one

        //    if (pass == 1)
        //    {
        //        if (passengerWaitTimes.Count > 0)
        //        {
        //            // Temp way to get min a value so it can determine what is smaller.

        //            //Aggregate the durations and then return the min value
        //            foreach (Duration d in passengerWaitTimes)
        //            {
        //                if (d > max)
        //                {
        //                    max = d;
        //                }
        //            }

        //            return max;
        //        }
        //        else
        //            return max;
        //    }
        //    else if (pass == 2)
        //    {
        //        if (passengerRideTimes.Count > 0)
        //        {
        //            // Temp way to get min a value so it can determine what is smaller.

        //            //Aggregate the durations and then return the min value
        //            foreach (Duration d in passengerRideTimes)
        //            {
        //                if (d > max)
        //                {
        //                    max = d;
        //                }
        //            }

        //            return max;
        //        }
        //        else
        //            return max;
        //    }
        //    else if (pass == 3)
        //    {
        //        if (passengerTotalTimes.Count > 0)
        //        {
        //            // Temp way to get min a value so it can determine what is smaller.

        //            //Aggregate the durations and then return the min value
        //            foreach (Duration d in passengerTotalTimes)
        //            {
        //                if (d > max)
        //                {
        //                    max = d;
        //                }
        //            }

        //            return max;
        //        }
        //        else
        //            return max;
        //    }
        //    else
        //        return max;
        //}


        public override string ToString()
        {
            return AllStatistics.ToDelimitedString("\n\n");
        }
    }
}