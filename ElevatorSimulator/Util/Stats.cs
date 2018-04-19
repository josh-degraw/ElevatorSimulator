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
        /// A representation of the wait times of the passenges in the application
        /// </summary>
        /// <value>
        /// The passenger wait times.
        /// </value>
        public DurationStatistic PassengerWaitTimes { get; } = new DurationStatistic("Passenger wait times");
        public DurationStatistic PassengerRideTimes { get; } = new DurationStatistic("Passenger ride times");
        public DurationStatistic PassengerTotalTimes { get; } = new DurationStatistic("Passenger ride times");
        public DurationStatistic TempTimes { get; } = new DurationStatistic("Temporary holder for times");


        private readonly ICollection<Duration> passengerWaitTimes;
        private readonly ICollection<Duration> passengerRideTimes; // still need to set all this up
        private readonly ICollection<Duration> passengerTotalTimes;// still need to set all this up 
        private ICollection<Duration> tempTimes;

        public static Stats Instance { get; } = new Stats();

        /// <summary>
        /// A representation of the wait times of the passenges in the application
        /// </summary>
        private Stats()
        {
            passengerWaitTimes = new Collection<Duration>();
            passengerRideTimes = new Collection<Duration>();
            passengerTotalTimes = new Collection<Duration>();
            tempTimes = new Collection<Duration>();
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
                passengerWaitTimes.Add(waitTime);
            
                passengerRideTimes.Add(rideTime);
           
                passengerTotalTimes.Add(totalTime);
            
        }
        // i need to potentially change this to take a duration and an int to determine which stat to add to
        // if int == 1 do the wait, it == 2 do the ride, etc
        // make sure these new functions are the right visibility


        // make the functions here usable by all types by passing in the kind you want

        /// <summary>
        /// This function returns a Duration representing the average wait time for passengers in the system.
        /// </summary>
        /// <returns></returns>
        public Duration GetAverageTime(int pass) 
        {

            Duration avg = new Duration(); 
            if (pass == 1)
            {
                if (passengerWaitTimes.Count > 0)
                {
                    //Aggregate the durations and then return an average
                    foreach (Duration d in passengerWaitTimes)
                    {
                        avg += d;
                    }
                    avg /= passengerWaitTimes.Count;
                    return avg;
                }
                else
                    return avg;
            }
            
            else  if (pass == 2)
            {
                if (passengerRideTimes.Count > 0)
                {
                    //Aggregate the durations and then return an average
                    foreach (Duration d in passengerRideTimes)
                    {
                        avg += d;
                    }
                    avg /= passengerRideTimes.Count;
                    return avg;
                }
                else
                    return avg;
            }

            else if (pass == 3)
            {
                if (passengerTotalTimes.Count > 0)
                {
                    //Aggregate the durations and then return an average
                    foreach (Duration d in passengerTotalTimes)
                    {
                        avg += d;
                    }
                    avg /= passengerTotalTimes.Count;
                    return avg;
                }
                else
                    return avg;
            }

            else
                return avg;
        }

        public Duration GetMinTime(int pass)
        {
            Duration max = new Duration();

            // for each min value check after the initial one

           
                if (pass == 1)
                {
                    tempTimes = passengerWaitTimes;
                }

                else if (pass == 2)
                {
                    tempTimes = passengerRideTimes;
                }

                else if (pass == 3)
                {
                    tempTimes = passengerTotalTimes;
                }


                if (tempTimes.Count > 0)
                {
                    // Temp way to get min a value so it can determine what is smaller.
                    min = GetMaxTime(pass);
                    //Aggregate the durations and then return the min value
                    foreach (Duration d in tempTimes)
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

        
        public Duration GetMaxTime(int pass)
        {
            Duration max = new Duration();

            // for each min value check after the initial one

            if (pass == 1)
            {
                if (passengerWaitTimes.Count > 0)
                {
                    // Temp way to get min a value so it can determine what is smaller.
                    
                    //Aggregate the durations and then return the min value
                    foreach (Duration d in passengerWaitTimes)
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

            else if (pass == 2)
            {
                if (passengerRideTimes.Count > 0)
                {
                    // Temp way to get min a value so it can determine what is smaller.
                   
                    //Aggregate the durations and then return the min value
                    foreach (Duration d in passengerRideTimes)
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

            else if (pass == 3)
            {
                if (passengerTotalTimes.Count > 0)
                {
                    // Temp way to get min a value so it can determine what is smaller.
                   
                    //Aggregate the durations and then return the min value
                    foreach (Duration d in passengerTotalTimes)
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