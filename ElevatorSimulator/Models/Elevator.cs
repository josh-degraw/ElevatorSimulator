using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Properties;
using ElevatorApp.Util;
using MoreLinq;
using NodaTime;
using static ElevatorApp.Util.Logger;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents an elevator.
    /// <para>Implements <see cref="IObserver{T}"/> to observe when a floor wants an elevator</para>
    /// </summary>
    public class Elevator : ModelBase, ISubcriber<ElevatorMasterController>, IObserver<(int, Direction)>
    {
        #region Backing fields

        private static int _RegisteredElevators = 0;
        private Direction _direction = Direction.None;
        private ElevatorState _elevatorState = ElevatorState.Idle;

        private int
            _speed = 800,
            _currentSpeed = 0,
            _totalCapacity,
            _currentFloor = 1;

        //[Obsolete]
        //private readonly AsyncObservableCollection<ElevatorCall> _path = new AsyncObservableCollection<ElevatorCall>();
        private readonly AsyncObservableCollection<Passenger> _passengers = new AsyncObservableCollection<Passenger>();
        private readonly AsyncObservableCollection<(int Floor, Direction Direction)> _floorsToStopAt = new AsyncObservableCollection<(int, Direction)>();

        #endregion

        #region Public properties

        #region INotifyParent Properties

        /// <summary>
        /// The total number of <see cref="Passenger"/>s the <see cref="Elevator"/> can hold
        /// </summary>
        public int TotalCapacity
        {
            get => _totalCapacity;
            set => SetProperty(ref _totalCapacity, value);
        }

        /// <summary>
        /// How fast the elevator is going
        /// <para>
        /// TODO: Set the speed when the elevator moves
        /// </para>
        /// </summary>
        public int Speed
        {
            get => _speed;
            private set
            {
                SetProperty(ref _speed, value);
                OnPropertyChanged(this.RelativeSpeed, nameof(this.RelativeSpeed));
            }
        }

        /// <summary>
        /// The relative speed of the <see cref="Elevator"/>. Negative values indicate downward movement.
        /// </summary>
        public int RelativeSpeed
        {
            get
            {
                switch (this.Direction)
                {
                    case Direction.None: return 0;
                    case Direction.Up: return this.Speed;
                    case Direction.Down: return -this.Speed;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public int CurrentSpeed
        {
            get => _currentSpeed;
            private set => SetProperty(ref _currentSpeed, value);
        }

        /// <summary>
        /// Represents the total weight currently held on the <see cref="Elevator"/>
        /// </summary>
        public int CurrentCapacity => Passengers?.Count ?? 0;

        /// <summary>
        /// The direction the elevator is going
        /// </summary>
        public Direction Direction
        {
            get => _direction;
            private set => SetProperty(ref _direction, value);
        }

        /// <summary>
        /// Represents the state of the elevator
        /// </summary>
        public ElevatorState State
        {
            get => _elevatorState;
            private set
            {
                Debug.Assert(_validateStateChange(_elevatorState, value), "Invalid state change for elevator", "{0} -> {1}", _elevatorState, value);
                SetProperty(ref _elevatorState, value);
            }
        }

        private bool _validateStateChange(ElevatorState prev, ElevatorState next)
        {
            if (prev == next)
                return true;

            switch (prev)
            {
                case ElevatorState.Idle:
                    return next == ElevatorState.Departing;

                case ElevatorState.Departing:
                    return next == ElevatorState.Departed;

                case ElevatorState.Departed:
                    return next == ElevatorState.Arriving;

                case ElevatorState.Arriving:
                    return next == ElevatorState.Arrived;

                case ElevatorState.Arrived:
                    return next.EqualsAny(ElevatorState.Departing, ElevatorState.Idle);

                default:
                    throw new ArgumentOutOfRangeException(nameof(prev), prev, null);
            }
        }

        /// <summary>
        /// Represents the current floor of the <see cref="Elevator"/>
        /// <para>
        /// </para>
        /// </summary>
        public int CurrentFloor
        {
            get => _currentFloor;
            private set
            {
                SetProperty(ref _currentFloor, value);
                OnPropertyChanged(this.NextFloor, nameof(this.NextFloor));
            }
        }

        /// <summary>
        /// Represents the closest elevator, If the <see cref="Elevator"/> is idle, this will be the same as <see cref="CurrentFloor"/>
        /// </summary>
        public int NextFloor
        {
            get
            {
                switch (Direction)
                {
                    case Direction.None: return this.CurrentFloor;
                    case Direction.Up: return this.CurrentFloor + 1;
                    case Direction.Down: return this.CurrentFloor - 1;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Other Properties

        /// <summary>
        /// The number of this <see cref="Elevator"/>. This is generated in order of initialization
        /// </summary>
        public int ElevatorNumber { get; }

        ///<inheritdoc/>
        public bool Subscribed { get; private set; }

        ///// <summary>
        ///// The <see cref="ElevatorCall"/>s that remain to be fulfilled by this <see cref="Elevator"/>.
        ///// </summary>
        //[Obsolete]
        //public IReadOnlyCollection<ElevatorCall> Path => _path;

        /// <summary>
        /// A collection of the floor numbers that want the <see cref="Elevator"/> to stop there
        /// </summary>
        public IReadOnlyCollection<(int Floor, Direction Direction)> FloorsToStopAt => _floorsToStopAt;

        /// <summary>
        /// The people currently inside the <see cref="Elevator"/>
        /// </summary>
        public IReadOnlyCollection<Passenger> Passengers => _passengers;

        /// <summary>
        /// The button panel inside of the elevator
        /// </summary>
        [Obsolete("For purposes of the program, this is being handled by the floors", true)]
        public ButtonPanel ButtonPanel { get; }

        /// <summary>
        /// The door of the <see cref="Elevator"/>
        /// </summary>
        public Door Door { get; } = new Door();

        #endregion

        #endregion

        #region Events
        /// <summary>
        /// Called when the <see cref="Elevator"/> is getting close to a <see cref="Floor"/>, but before it's officially started arriving. After this event is handled, 
        /// </summary>
        public event EventHandler<ElevatorApproachingEventArgs> Approaching;

        /// <summary>
        /// Called when the elevator is about to arrive
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Arriving;

        /// <summary>
        /// Called when the <see cref="Elevator"/> has arrived, right before the doors open
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Arrived;

        /// <summary>
        /// Called when the <see cref="Elevator"/> is about to leave, right after the doors have closed
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Departing;

        /// <summary>
        /// Called when the <see cref="Elevator"/> has just left
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Departed;

        /// <summary>
        /// Called when a <see cref="Passenger"/> has just entered the <see cref="Elevator"/>
        /// </summary>
        public event EventHandler<Passenger> PassengerAdded = (_, p) => LogEvent("Passenger Added to Elevator", parameters: p);

        /// <summary>
        /// Called when a <see cref="Passenger"/> has just left the <see cref="Elevator"/>
        /// </summary>
        public event EventHandler<Passenger> PassengerExited = (_, p) => LogEvent("Passenger Exited Elevator", parameters: p);

        #endregion

        #region Speed values

        /// <summary>
        /// The time it takes to move from one floor to another
        /// </summary>
        public static readonly Duration FLOOR_MOVEMENT_SPEED = Duration.FromSeconds(7);

        /// <summary>
        /// The time it takes to get up to full speed once departing
        /// </summary>
        public static readonly Duration ACCELERATION_DELAY = Duration.FromSeconds(4);

        /// <summary>
        /// The time it takes to slow down before arriving
        /// </summary>
        public static readonly Duration DECELERATION_DELAY = ACCELERATION_DELAY * 2;

        private readonly AsyncObservableCollection<IObserver<int>> _observers = new AsyncObservableCollection<IObserver<int>>();

        /// <summary>
        /// Same as <see cref="FLOOR_MOVEMENT_SPEED"/>, but in a format that can be used by the GUI
        /// </summary>
        public static System.Windows.Duration FloorMovementSpeed => new System.Windows.Duration(FLOOR_MOVEMENT_SPEED.ToTimeSpan());
        #endregion

        #region Initialization / Finalizaton
        /// <summary>
        /// Instantiates a new <see cref="Elevator"/>
        /// </summary>
        /// <param name="initialFloor">The floor to start on</param>
        public Elevator(int initialFloor = 1)
        {
            this.ElevatorNumber = ++_RegisteredElevators;

            Logger.LogEvent("Initializing Elevator", ("ElevatorNumber", this.ElevatorNumber));

            this._floorsToStopAt.CollectionChanged += (sender, args) =>
            {
                Logger.LogEvent("Elevator Queue changed", ("Path", _floorsToStopAt.OrderBy(a => a).ToDelimitedString(",")));
                base.OnPropertyChanged(FloorsToStopAtString, nameof(FloorsToStopAtString));
            };

            this.Arriving += ElevatorArriving;
            this.Arrived += ElevatorArrived;

            this.Departing += ElevatorDeparting;
            this.Departed += ElevatorDeparted;
        }

        /// <summary>
        /// A string representation of the floors that are currently in the queue
        /// </summary>
        public string FloorsToStopAtString => FloorsToStopAt.ToDelimitedString(", ");

        /// <summary>
        /// Finalizer. Decrements the global count of elevators
        /// </summary>
        ~Elevator()
        {
            // Decrement the global count of elevators
            if (Elevator._RegisteredElevators > 0)
                Elevator._RegisteredElevators -= 1;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the <see cref="Departing"/> event. Sets the state of the elevator to the direction it will be moving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorDeparting(object sender, ElevatorMovementEventArgs args)
        {
            if (this.CurrentFloor == args.DestinationFloor)
                return;

            lock (this._lock)
            {
                this.State = ElevatorState.Departing;
                LogEvent("Elevator Departing", (this.CurrentFloor, args.DestinationFloor));
            }
        }

        /// <summary>
        /// Handles the <see cref="Departed"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorDeparted(object sender, ElevatorMovementEventArgs args)
        {
            if (args.DestinationFloor == this.CurrentFloor)
                return;

            lock (this._lock)
            {
                this.State = ElevatorState.Departed;
                LogEvent("Elevator Departed", (this.CurrentFloor, args.DestinationFloor));
            }
        }

        /// <summary>
        /// Handles the <see cref="Arriving"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorArriving(object sender, ElevatorMovementEventArgs args)
        {
            lock (this._lock)
            {
                this.State = ElevatorState.Arriving;
                LogEvent("Elevator Arriving", ("Floor", args));
            }
        }
        private readonly SoundPlayer soundPlayer = new SoundPlayer { Stream = Resources.elevatorDing };

        /// <summary>
        /// Handles the <see cref="Arrived"/> event. Logs a message, and removes the passengers that are getting off at the given floor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">The floor that the elevator is arriving at</param>
        private async void ElevatorArrived(object sender, ElevatorMovementEventArgs args)
        {
            try
            {
                _floorsToStopAt.TryRemove((args.DestinationFloor, args.Direction));
                this.CurrentFloor = args.DestinationFloor;
                this.State = ElevatorState.Arrived;
                soundPlayer.Play();
                LogEvent("Elevator Arrived", ("Floor", args.DestinationFloor));

                IEnumerable<Passenger> leaving = this.Passengers.Where(p => p.Path.destination == args.DestinationFloor);
                await WaitForDoorToOpen();

                foreach (Passenger passenger in leaving)
                {
                    try
                    {
                        await this.RemovePassenger(passenger).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                //Check to see if the elevator still needs to move and everyones gone(ie waiting on another floor). If it DOES, then dispatch the elevator to the nearest destination
                if (this.FloorsToStopAt.Any() && !this.Passengers.Any())
                {
                    //Reset the direction and then get the next floor to pick up a passenger at
                    this.Direction = this.assignDirection(FloorsToStopAt.Min(a => Math.Abs(a.Floor - this.CurrentFloor)));
                    await this.Move().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new <see cref="Passenger"/> to the <see cref="Elevator"/>
        /// </summary>
        /// <param name="passenger">The <see cref="Passenger"/> that will be boarding.</param>
        public async Task AddPassenger(Passenger passenger)
        {
            LogEvent("Adding Passenger to Elevator");
            passenger.State = PassengerState.Transition;


            // Simulate time it takes to enter the elevator
            await Task.Delay(Passenger.TransitionSpeed);
            passenger.State = PassengerState.In;

            this._passengers.AddDistinct(passenger);

            PassengerAdded?.Invoke(this, passenger);

            var path = (passenger.Path.destination, passenger.Direction);

            if (!this.FloorsToStopAt.Contains(path))
            {
                this._floorsToStopAt.Add(path);
            }
        }

        /// <summary>
        /// Moves a <see cref="Passenger"/> off of the <see cref="Elevator"/>
        /// </summary>
        /// <param name="passenger">The <see cref="Passenger"/> that will be departing.</param>
        private async Task RemovePassenger(Passenger passenger)
        {
            LogEvent("Passenger Leaving Elevator");
            passenger.State = PassengerState.Transition;
            await Task.Delay(Passenger.TransitionSpeed);

            this._passengers.Remove(passenger);
            passenger.State = PassengerState.Out;
            PassengerExited?.Invoke(this, passenger);
        }


        /// <summary>
        /// Wires up this <see cref="T:ElevatorApp.Models.Elevator" /> to the given <see cref="T:ElevatorApp.Models.ElevatorMasterController" />
        /// </summary>
        public async Task Subscribe(ElevatorMasterController controller)
        {
            if (Subscribed)
                return;

            Logger.LogEvent("Subcribing elevator to MasterController", ("Elevator Number", this.ElevatorNumber.ToString()));

            await this.Door.Subscribe(this).ConfigureAwait(false);
            this.Subscribed = true;
        }

        private Direction assignDirection(int destination)
        {
            if (this.CurrentFloor == destination)
                return Direction.None;

            else if (this.CurrentFloor < destination)
                return Direction.Up;

            else
                return Direction.Down;
        }

        /// <summary>
        /// Adds a floor number to this <see cref="Elevator"/>, and, if the <see cref="Elevator"/> is currently idle, starts up the process.
        /// <para>
        /// If the <see cref="Elevator"/> is already at the <see cref="Floor"/> that has been requested, trigger the <see cref="Arrived"/> event and not process any further
        /// </para>
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        internal async Task Dispatch((int, Direction) destination)
        {
            if (!_floorsToStopAt.Contains(destination))
                _floorsToStopAt.AddDistinct(destination);

            if (!_moving && this.Direction == Direction.None)
            {
                // Starts the movement in a seperate thread with Task.Run
                await this.Move().ConfigureAwait(false);
            }
        }

        private bool _moving = false;

        /// <summary>
        /// Lock object to help with synchronization
        /// </summary>
        private readonly object _lock = new object();


        private async Task WaitForDoorToClose()
        {
            //Wait for the door to be closed
            while (this.Door.DoorState != DoorState.Closed)
                await Task.Delay(1);
        }
        private async Task WaitForDoorToOpen()
        {
            //Wait for the door to be closed
            while (this.Door.DoorState != DoorState.Opened)
                await Task.Delay(1);
        }

        /// <summary>
        /// Handles the logic for the movement of the <see cref="Elevator"/>.
        /// Loops through the <see cref="FloorsToStopAt"/> and moves the <see cref="Elevator"/> accordingly
        /// </summary>
        private async Task Move()
        {
            try
            {
                await WaitForDoorToClose();

                // Get the closest floor to the current
                if (NextFloor == this.CurrentFloor)
                {
                    // If this is here, the elevator is not moving.
                    // If so, if there are any floors requested, go to the nearest one
                }

                while (this._floorsToStopAt.Any())
                {
                    _moving = true;

                    var (destination, direction) = FloorsToStopAt
                        .Where(a =>
                        {
                            if (this.Direction == Direction.None)
                            {
                                return a.Floor != this.CurrentFloor;
                            }

                            return this.Direction == Direction.None || a.Direction == this.Direction && a.Floor != this.CurrentFloor;
                        })
                        .MinBy(a => Math.Abs(a.Floor - this.CurrentFloor));

                    this.Direction = direction; //assignDirection(destination.Floor);
                    if (this.Direction == Direction.None)
                        break;

                    var call = new ElevatorMovementEventArgs(destination, direction);

                    await WaitForDoorToClose();

                    await _startMovement(call);

                    switch (this.Direction)
                    {
                        case Direction.Up:
                            Trace.Indent();
                            for (int i = this.CurrentFloor; i < destination; i++)
                            {
                                await Task.Delay(FLOOR_MOVEMENT_SPEED.ToTimeSpan());
                                int nextFloor = i + 1;

                                var args = new ElevatorApproachingEventArgs(nextFloor, destination, call.Direction);

                                // If the elevator should stop at this floor
                                bool shouldStop = false;
                                lock (_lock)
                                    shouldStop = _approachFloor(args);

                                if (shouldStop)
                                {
                                    LogEvent($"Elevator approaching floor {nextFloor}");
                                    await _arriveAtFloor(new ElevatorMovementEventArgs(nextFloor, call.Direction));

                                    // Start moving to the next floor
                                    ++i;
                                    call = new ElevatorMovementEventArgs(nextFloor, call.Direction);
                                    await _startMovement(call);
                                }
                                else if (nextFloor != destination)
                                {
                                    this.CurrentFloor = nextFloor; // Set the current floor
                                }
                            }
                            Trace.IndentLevel--;

                            break;

                        case Direction.Down:
                            Trace.Indent();
                            for (int i = this.CurrentFloor; i > destination; i--)
                            {
                                await Task.Delay(FLOOR_MOVEMENT_SPEED.ToTimeSpan());
                                int nextFloor = i - 1;

                                var args = new ElevatorApproachingEventArgs(nextFloor, destination, call.Direction);

                                // If the elevator should stop at this floor
                                if (_approachFloor(args))
                                {
                                    await _arriveAtFloor(new ElevatorMovementEventArgs(nextFloor, call.Direction));

                                    // Start moving to the next floor
                                    --i;
                                    call = new ElevatorMovementEventArgs(nextFloor - 1, call.Direction);
                                    await _startMovement(call);
                                }
                                else if (nextFloor != destination)
                                {
                                    this.CurrentFloor = nextFloor; // Set the current floor
                                }
                            }
                            Trace.IndentLevel--;

                            break;
                    }

                    await _arriveAtFloor(call);
                }

                _moving = false;
                this.Direction = Direction.None;
                this.State = ElevatorState.Idle;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// When the elevator is about to get near a floor
        /// </summary>
        /// <param name="args"></param>
        /// <returns><see langword="true"/> if the <see cref="Elevator"/> should stop</returns>
        private bool _approachFloor(ElevatorApproachingEventArgs args)
        {
            LogEvent($"Elevator approaching floor {args.IntermediateFloor}");

            // TODO: If any passengers have called to here, let them get on
            lock (_lock)
                this.Approaching?.Invoke(this, args);

            return args.ShouldStop;
        }

        /// <summary>
        /// Stars moving
        /// </summary>
        /// <param name="args"></param>
        private async Task _startMovement(ElevatorMovementEventArgs args)
        {
            if (args.Handled)
                return;
            try
            {
                await WaitForDoorToClose();
                lock (_lock)
                    this.Departing?.Invoke(this, args);

                await Task.Delay(ACCELERATION_DELAY.ToTimeSpan());

                lock (_lock)
                    this.Departed?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Handles arriving at a floor
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task _arriveAtFloor(ElevatorMovementEventArgs args)
        {
            try
            {
                if (this.State == ElevatorState.Departed)
                {
                    lock (_lock)
                    {
                        this.Arriving?.Invoke(this, args);
                    }

                    await Task.Delay(DECELERATION_DELAY.ToTimeSpan());
                }

                if (this.State == ElevatorState.Arriving)
                {
                    lock (_lock)
                    {
                        this.Arrived?.Invoke(this, args);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion

        public async void OnNext((int, Direction) value)
        {
            await this.Dispatch(value).ConfigureAwait(false);
        }

        public void OnError(Exception error)
        {
            ;// Not implemented
        }

        public void OnCompleted()
        {
            ;// Not implemented
        }
    }
}
