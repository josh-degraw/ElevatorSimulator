using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a Passenger
    /// </summary>
    /// <seealso cref="ElevatorApp.Models.ModelBase" />
    public class Passenger : ModelBase
    {
        /// <summary>
        /// The total passenger count.
        /// </summary>
        private static int TotalPassengerCount;

        #region Timings

        /// <summary>
        /// The instant the passenger was created and therefore started waiting for the elevator
        /// </summary>
        private Instant WaitStart = LocalSystemClock.GetCurrentInstant();

        /// <summary>
        /// The instant the <see cref="Elevator"/> arrived and the <see cref="Passenger"/>'s state changed to <see cref="PassengerState.Transition"/>
        /// </summary>
        private Instant WaitEnd = default;

        /// <summary>
        /// The instant that the <see cref="Passenger"/> entered the <see cref="Elevator"/>
        /// </summary>
        private Instant Entered = default;

        /// <summary>
        /// The instant that the <see cref="Passenger"/> exited the <see cref="Elevator"/>
        /// </summary>
        private Instant Exited = default;


        /// <summary>
        /// Represents how long the <see cref="Passenger"/> spent waiting for the <see cref="Elevator"/> to come
        /// </summary>
        public Duration TimeWaiting => WaitEnd - WaitStart;

        /// <summary>
        /// Represents how long the <see cref="Passenger"/> spent in the <see cref="Elevator"/>
        /// </summary>
        public Duration TimeSpentInElevator => Exited - Entered;

        #endregion

        /// <summary>
        /// Represents how heavy the <see cref="Passenger"/> is
        /// </summary>
        public int Weight { get; set; }

        private PassengerState _state = PassengerState.Waiting;

        /// <summary>
        /// Represents the state of the <see cref="Passenger"/>
        /// </summary>
        public PassengerState State
        {
            get => _state;
            set
            {
                PassengerState oldVal = _state;
                SetProperty(ref _state, value);

                switch (value)
                {
                    case PassengerState.Waiting:
                        WaitStart = LocalSystemClock.GetCurrentInstant();
                        break;
                    case PassengerState.Transition when oldVal == PassengerState.Waiting:
                        WaitEnd = LocalSystemClock.GetCurrentInstant();
                        break;
                    case PassengerState.In:
                        Entered = LocalSystemClock.GetCurrentInstant();
                        break;
                    case PassengerState.Out:
                        Exited = LocalSystemClock.GetCurrentInstant();
                        break;
                    default:
                        break;
                }
            }
        }

        private (int source, int destination) _path;


        /// <summary>
        /// A tuple representing the path of the <see cref="Passenger"/>
        /// </summary>
        public (int source, int destination) Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }


        /// <summary>
        /// Computed property representing the direction the passenger wants to go
        /// </summary>
        public Direction Direction => this.Path.destination > this.Path.source ? Direction.Up : Direction.Down;

        /// <summary>
        /// Represents the intended floor and the direction the passenter wants to go
        /// </summary>
        public (int floor, Direction direction ) Call => (this.Path.destination, this.Direction);

        /// <summary>
        /// The amount of time it takes for this <see cref="Passenger"/> to transition into or out of the <see cref="Elevator"/>
        /// </summary>
        public static readonly TimeSpan TransitionSpeed = TimeSpan.FromSeconds(1.5);

        /// <summary>
        /// The number of this <see cref="Passenger"/>, in the order of creation
        /// </summary>
        public int PassengerNumber { get; }

        /// <summary>
        /// Creates a new passenger, incrementing the total number of passengers that have been created
        /// Only for use by the designer
        /// </summary>
        public Passenger()
        {
            this.PassengerNumber = ++TotalPassengerCount;
        }

        /// <summary>
        /// Constructs a new passenger with the given path
        /// </summary>
        /// <param name="source">The floor that the passenger is coming from</param>
        /// <param name="destination">The floor that the passenger is going to</param>
        public Passenger(int source, int destination) : this()
        {
            if (source == destination)
                throw new ArgumentException("Source and destination must not be the same.");

            this.Path = (source, destination);
        }

        /// <summary>
        /// String representation of how long the <see cref="Passenger"/> has been waiting
        /// </summary>
        /// <returns></returns>
        public string GetWaitingString()
        {
            if (WaitEnd == default)
                return "";

            return $"Time Spent Waiting: {TimeWaiting:mm:ss}";
        }


        /// <summary>
        /// String representation of how long the <see cref="Passenger"/> spent in the <see cref="Elevator"/>
        /// </summary>
        public string GetTimeInElevatorString()
        {
            if (Exited == default)
                return "";

            return $"Time Spent in Elevator: {TimeSpentInElevator:mm:ss}";
        }

        /// <summary>
        /// String representation of this <see cref="Passenger"/>'s path
        /// </summary>
        public override string ToString() => $"Path: {this.Path.source} -> {this.Path.destination}";


        /// <summary>
        /// Implicitly convert this <see cref="Passenger"/> to an array of <see cref="Event"/> parameters
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public static implicit operator (object, object)[] (Passenger passenger)
        {
            const string duration_format = "mm:ss";

            var obj = new List<(object, object)>(3)
            {
                ("Path", $"{passenger.Path.source} -> {passenger.Path.destination}")
            };

            if (passenger.WaitEnd != default)
                obj.Add(("Time Spent Waiting", passenger.TimeWaiting.ToString(duration_format, null)));

            if (passenger.Exited != default)
            {
                obj.Add(("Time Spent in Elevator", passenger.TimeSpentInElevator.ToString(duration_format, null)));
                //This is where I have the passenger add its timings to the stats class for now... certainly a temporary thing
                Stats.Instance.PassengerWaitTimes.Add(passenger.TimeWaiting);
                //I also have it print out
                obj.Add(("Current Average Wait Time ", Stats.Instance.PassengerWaitTimes));
                //End this section
            }

            return obj.ToArray();
        }
    }
}
