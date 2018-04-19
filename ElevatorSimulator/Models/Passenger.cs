using ElevatorApp.Models.Enums;
using ElevatorApp.Util;
using NodaTime;
using System;
using System.Collections.Generic;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a Passenger
    /// </summary>
    /// <seealso cref="ElevatorApp.Models.ModelBase"/>
    public class Passenger : ModelBase
    {
        #region Private Fields

        /// <summary>
        /// The total passenger count.
        /// </summary>
        private static int TotalPassengerCount;

        private (int source, int destination) _path;

        private PassengerState _state = PassengerState.Waiting;

        #region Timings

        /// <summary>
        /// The instant that the <see cref="Passenger"/> entered the <see cref="Elevator"/>
        /// </summary>
        private Instant Entered = default;

        /// <summary>
        /// The instant that the <see cref="Passenger"/> exited the <see cref="Elevator"/>
        /// </summary>
        private Instant Exited = default;

        /// <summary>
        /// The instant the <see cref="Elevator"/> arrived and the <see cref="Passenger"/>'s state changed to <see cref="PassengerState.Transition"/>
        /// </summary>
        private Instant WaitEnd = default;

        /// <summary>
        /// The instant the passenger was created and therefore started waiting for the elevator
        /// </summary>
        private Instant WaitStart = LocalSystemClock.GetCurrentInstant();

        /// <summary>
        ///  Represents the total time a person is waiting and using the elevator
        /// </summary>
        public Duration TotalTime => TimeWaiting + TimeSpentInElevator;


        #endregion

        #endregion Private Fields

        #region Public Fields

        /// <summary>
        /// The amount of time it takes for this <see cref="Passenger"/> to transition into or out of the <see cref="Elevator"/>
        /// </summary>
        public static readonly TimeSpan TransitionSpeed = TimeSpan.FromSeconds(1.5);

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Creates a new passenger, incrementing the total number of passengers that have been created Only for use by
        /// the designer
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

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Represents the intended floor and the direction the passenter wants to go
        /// </summary>
        public (int floor, Direction direction) Call => (this.Path.destination, this.Direction);

        /// <summary>
        /// The number of this <see cref="Passenger"/>, in the order of creation
        /// </summary>
        public int PassengerNumber { get; }

        /// <summary>
        /// A tuple representing the path of the <see cref="Passenger"/>
        /// </summary>
        public (int source, int destination) Path
        {
            get => this._path;
            set => this.SetProperty(ref this._path, value);
        }

        /// <summary>
        /// Represents the state of the <see cref="Passenger"/>
        /// </summary>
        public PassengerState State
        {
            get => this._state;

            set
            {
                PassengerState oldVal = this._state;
                this.SetProperty(ref this._state, value);

                switch (value)
                {
                    case PassengerState.Waiting:
                        this.WaitStart = LocalSystemClock.GetCurrentInstant();
                        break;

                    case PassengerState.Transition when oldVal == PassengerState.Waiting:
                        this.WaitEnd = LocalSystemClock.GetCurrentInstant();
                        break;

                    case PassengerState.In:
                        this.Entered = LocalSystemClock.GetCurrentInstant();
                        break;

                    case PassengerState.Out:
                        this.Exited = LocalSystemClock.GetCurrentInstant();
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Represents how long the <see cref="Passenger"/> spent in the <see cref="Elevator"/>
        /// </summary>
        public Duration TimeSpentInElevator => this.Exited - this.Entered;

        /// <summary>
        /// Represents how long the <see cref="Passenger"/> spent waiting for the <see cref="Elevator"/> to come
        /// </summary>
        public Duration TimeWaiting => this.WaitEnd - this.WaitStart;

        /// <summary>
        /// Represents how heavy the <see cref="Passenger"/> is
        /// </summary>
        public int Weight { get; set; }


        // add the timespent + timewaiting to get total time for a person.
        // then send all of those 3 stats over to the add

        /// <summary>
        /// Computed property representing the direction the passenger wants to go
        /// </summary>
        public Direction Direction => this.Path.destination > this.Path.source ? Direction.Up : Direction.Down;

        #endregion Public Properties

        #region Public Methods

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
                
                // This is where the 3 statistics are added to the stats collections
                Stats.Instance.PassengerWaitTimes.Add(passenger.TimeWaiting);
                Stats.Instance.PassengerRideTimes.Add(passenger.TimeSpentInElevator);
                Stats.Instance.PassengerWaitTimes.Add(passenger.TotalTime);

                
                //I also have it print out
                obj.Add(("Current Average Wait Time ", Stats.Instance.PassengerWaitTimes));

                //End this section
            }

            return obj.ToArray();
        }

        /// <summary>
        /// String representation of how long the <see cref="Passenger"/> spent in the <see cref="Elevator"/>
        /// </summary>
        public string GetTimeInElevatorString()
        {
            if (this.Exited == default)
                return "";

            return $"Time Spent in Elevator: {this.TimeSpentInElevator:mm:ss}";
        }

        /// <summary>
        /// String representation of how long the <see cref="Passenger"/> has been waiting
        /// </summary>
        /// <returns></returns>
        public string GetWaitingString()
        {
            if (this.WaitEnd == default)
                return "";

            return $"Time Spent Waiting: {this.TimeWaiting:mm:ss}";
        }

        /// <summary>
        /// String representation of this <see cref="Passenger"/>'s path
        /// </summary>
        public override string ToString() => $"Path: {this.Path.source} -> {this.Path.destination}";

        #endregion Public Methods
    }
}