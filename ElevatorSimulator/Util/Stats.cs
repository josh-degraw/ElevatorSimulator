using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace ElevatorApp.Util
{
    /// <summary>
    /// The Stats class. This class will be able to hold and calculate information about the test during runtime. EG Average wait...
    /// </summary>
    class Stats
    {

        public DurationStatistic PassengerWaitTimes { get; } = new DurationStatistic("Passenger wait times");

        private readonly ICollection<Duration> passengerWaitTimes;
        public static Stats Instance { get; } = new Stats();

        private Stats()
        {
            passengerWaitTimes = new Collection<Duration>();
        }

        /// <summary>
        /// This function takes a Duration value and adds it to the collection of passenger wait times. It should be used whenever a passenger exits an elevator
        /// </summary>
        /// <param name="time"></param>
        public void AddPassengerTime(Duration time)
        {
            passengerWaitTimes.Add(time);
        }

        /// <summary>
        /// This function returns a Duration representing the average wait time for passengers in the system.
        /// </summary>
        /// <returns></returns>
        public Duration GetAverageWaitTime()
        {
            Duration avg = new Duration();
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

        /// <summary>
        /// This returns the number of passengers that have passed through the system and completed their trip
        /// </summary>
        /// <returns></returns>
        public int GetPassengerCount()
        {
            return passengerWaitTimes.Count;
        }
    }
}
