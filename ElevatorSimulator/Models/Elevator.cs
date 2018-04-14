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
            _currentFloor = 1,
            _nextFloor = 1;

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
            get
            {
                if (this.CurrentFloor == this.NextFloor)
                {
                    return Direction.None;
                }
                else if (this.NextFloor > this.CurrentFloor)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Down;
                }
            }
        }
        //{
        //    get => _direction;
        //    private set => SetProperty(ref _direction, value);
        //}

        /// <summary>
        /// The direction the elevator has been requested to go once it arrives
        /// </summary>
        public Direction RequestedDirection
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
            private set => SetProperty(ref _currentFloor, value.KeepInRange(0, 4)); //TODO: remove hardcoded floor number
        }

        /// <summary>
        /// Represents the closest elevator, If the <see cref="Elevator"/> is idle, this will be the same as <see cref="CurrentFloor"/>
        /// </summary>
        public int NextFloor
        {
            get => _nextFloor;
            private set => SetProperty(ref _nextFloor, value.KeepInRange(0, 4)); //TODO: remove hardcoded floor number
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
        public IReadOnlyCollection<(int Floor, Direction Direction)> FloorRequests => _floorsToStopAt;

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

            this.Departing -= ElevatorDeparting;
            this.Departed -= ElevatorDeparted;

            this.Departing += ElevatorDeparting;
            this.Departed += ElevatorDeparted;
        }

        /// <summary>
        /// A string representation of the floors that are currently in the queue
        /// </summary>
        public string FloorsToStopAtString => FloorRequests.ToDelimitedString(", ");

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

            this.State = ElevatorState.Departing;
            LogEvent("Elevator Departing", (this.CurrentFloor, args.DestinationFloor));

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

            this.State = ElevatorState.Departed;
            LogEvent("Elevator Departed", (this.CurrentFloor, args.DestinationFloor));

        }

        /// <summary>
        /// Handles the <see cref="Arriving"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorArriving(object sender, ElevatorMovementEventArgs args)
        {
            this.State = ElevatorState.Arriving;
            LogEvent("Elevator Arriving", ("Floor", args));

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
                this.State = ElevatorState.Arrived;
                _floorsToStopAt.TryRemove((args.DestinationFloor, args.Direction));

                this.CurrentFloor = args.DestinationFloor;

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

                //Check to see if the elevator still needs to move and everyones gone (ie waiting on another floor). If it DOES, then dispatch the elevator to the nearest destination
                if (!_moving && this.FloorRequests.Any() && !this.Passengers.Any())
                {
                    //Reset the direction and then get the next floor to pick up a passenger at
                    this.RequestedDirection = this.assignDirection(FloorRequests.Min(a => Math.Abs(a.Floor - this.CurrentFloor)));
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

            this.OnNext(passenger.Call);
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
            Stats.Instance.AddPassengerTime(passenger.TimeSpentInElevator + passenger.TimeWaiting);
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
        private async Task Dispatch((int floor, Direction direction) destination)
        {
            if (!_floorsToStopAt.Contains(destination))
                _floorsToStopAt.AddDistinct(destination);

            if (!_moving && this.Direction == Direction.None)
            {
                this.RequestedDirection = destination.direction;
                // Starts the movement in a seperate thread with Task.Run
                await Task.Run(this.Move).ConfigureAwait(false);
            }
        }

        private bool _moving = false;


        private async Task WaitForDoorToClose()
        {
            if (this.Door.DoorState == DoorState.Closed)
                return;

            while (this.Door.DoorState != DoorState.Closed)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        }

        private async Task WaitForDoorToOpen()
        {
            if (this.Door.DoorState == DoorState.Opened)
                return;

            while (this.Door.DoorState != DoorState.Opened)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        }

        private async Task PerformMovement(int destination, Direction direction, Func<int, int> floorIncrementer)
        {
            await Task.Delay(FLOOR_MOVEMENT_SPEED.ToTimeSpan()).ConfigureAwait(false);

            var args = new ElevatorApproachingEventArgs(this.NextFloor, destination, direction);

            // If the elevator should stop at this floor
            bool shouldStop = _approachFloor(args);

            if (shouldStop)
            {

                var call = new ElevatorMovementEventArgs(this.NextFloor, direction);
                await _arriveAtFloor(call);

                this.NextFloor = floorIncrementer(this.CurrentFloor);

                await _startMovement(call);
                // Start moving to the next floor
            }

            else if (this.NextFloor != destination)
            {
                this.CurrentFloor = this.NextFloor; // Set the current floor
            }

        }

        /// <summary>
        /// Handles the logic for the movement of the <see cref="Elevator"/>.
        /// Loops through the <see cref="FloorRequests"/> and moves the <see cref="Elevator"/> accordingly
        /// </summary>
        private async Task Move()
        {
            SemaphoreSlim mutex = new SemaphoreSlim(1);
            await mutex.WaitAsync();
            try
            {
                while (this._floorsToStopAt.Any())
                {
                    var (destination, direction) =
                        FloorRequests
                            .Where(a =>
                            {
                                // If we're not moving, find the closest floor.
                                if (this.Direction == Direction.None)
                                {
                                    return a.Floor != this.CurrentFloor;
                                }

                                // If we are moving, find the closest floor that is not the current floor
                                return a.Direction == this.Direction && a.Floor != this.CurrentFloor;
                            })
                            .MinBy(a => Math.Abs(a.Floor - this.CurrentFloor));

                    this.RequestedDirection = assignDirection(destination);

                    if (this.RequestedDirection == Direction.None)
                        break;


                    switch (this.RequestedDirection)
                    {
                        case Direction.Up:

                            this.NextFloor = this.CurrentFloor + 1;
                            await _startMovement(new ElevatorMovementEventArgs(destination, direction));

                            do
                            {
                                this.NextFloor = this.CurrentFloor + 1;
                                await PerformMovement(destination, direction, a => a + 1);
                            }
                            while (this.NextFloor < destination);


                            break;

                        case Direction.Down:
                            Trace.Indent();
                            this.NextFloor = this.CurrentFloor - 1;
                            await _startMovement(new ElevatorMovementEventArgs(destination, direction));

                            do
                            {
                                this.NextFloor = this.CurrentFloor - 1;
                                await PerformMovement(destination, direction, a => a - 1);
                            }
                            while (this.NextFloor > destination);

                            break;
                    }

                    await _arriveAtFloor(new ElevatorMovementEventArgs(destination, direction));
                }

                _moving = false;
                this.RequestedDirection = Direction.None;
                this.State = ElevatorState.Idle;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                mutex.Release();
            }
        }

        /// <summary>
        /// When the elevator is about to get near a floor
        /// </summary>
        /// <param name="args"></param>
        /// <returns><see langword="true"/> if the <see cref="Elevator"/> should stop</returns>
        private bool _approachFloor(ElevatorApproachingEventArgs args)
        {
            LogEvent("Elevator approaching ", ("Floor", args.IntermediateFloor), ("Eventual Destination", args.DestinationFloor));

            // TODO: If any passengers have called to here, let them get on

            this.Approaching?.Invoke(this, args);

            if (args.ShouldStop)
            {
                LogEvent("Elevator intercepted", ("Interceptor", args.IntermediateFloor));
            }
            return args.ShouldStop;
        }

        /// <summary>
        /// Stars moving
        /// </summary>
        /// <param name="args"></param>
        private async Task _startMovement(ElevatorMovementEventArgs args)
        {
            try
            {
                await WaitForDoorToClose().ConfigureAwait(false);

                this._moving = true;

                this.Departing?.Invoke(this, args);

                await Task.Delay(ACCELERATION_DELAY.ToTimeSpan()).ConfigureAwait(false);

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
                if (this.State == ElevatorState.Departed || this.State == ElevatorState.Departing)
                {
                    this.Arriving?.Invoke(this, args);

                    await Task.Delay(DECELERATION_DELAY.ToTimeSpan()).ConfigureAwait(false);
                }

                if (this.State == ElevatorState.Arriving)
                {
                    this.Arrived?.Invoke(this, args);
                }
                else
                {
                    //Debug.Fail("Invalid state", "Must be in state of Arriving to trigger Arrived event");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion

        /// <summary>
        /// Tells the elevator that a new call has been made
        /// </summary>
        /// <param name="value"></param>
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
