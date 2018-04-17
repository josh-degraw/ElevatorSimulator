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
    public class Elevator : ModelBase, ISubcriber<ElevatorMasterController>, IObserver<(int floor, Direction direction)>
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        // private readonly ReaderWriterLockSlim _passengerLock = new ReaderWriterLockSlim();
        //private readonly ReaderWriterLockSlim _floorsLock = new ReaderWriterLockSlim();

        private readonly object _locker = new object();

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
        /// The direction the elevator is `actually` going.
        /// <para>
        /// i.e. if the elevator is on floor 1, and someone on floor 3 requests to go down, 
        /// once moving, <see cref="RequestedDirection"/> will be <see cref="Enums. Direction.Down"/>
        /// while <see cref="Direction"/> will be <see cref="Enums.Direction.Up"/>,
        /// </para>
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
        /// 
        /// <para>
        /// i.e. if the elevator is on floor 1, and someone on floor 3 requests to go down, 
        /// once moving, <see cref="RequestedDirection"/> will be <see cref="Enums. Direction.Down"/>
        /// while <see cref="Direction"/> will be <see cref="Enums.Direction.Up"/>,
        /// </para>
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
            private set => SetProperty(ref _currentFloor, value.KeepInRange(0, this._getFloorNum())); //TODO: remove hardcoded floor number
        }

        /// <summary>
        /// Represents the closest elevator, If the <see cref="Elevator"/> is idle, this will be the same as <see cref="CurrentFloor"/>
        /// </summary>
        public int NextFloor
        {
            get => _nextFloor;
            private set => SetProperty(ref _nextFloor, value.KeepInRange(0, this._getFloorNum()));
        }

        /// <summary>
        /// Get the number of floors
        /// </summary>
        private Func<int> _getFloorNum = () => 4;

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
        public event EventHandler<Passenger> PassengerAdded;
        //    = (_, p) =>
        //{
        //    LogEvent("Passenger Added to Elevator", parameters: p);

        //};

        /// <summary>
        /// Called when a <see cref="Passenger"/> has just left the <see cref="Elevator"/>
        /// </summary>
        public event EventHandler<Passenger> PassengerExited;
        //    = (_, p) =>
        //{
        //    try
        //    {
        //        LogEvent("Passenger Exited Elevator", parameters: p);
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //};

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
            LogEvent("Elevator Departing", (this.CurrentFloor, args.DestinationFloor));
        }

        /// <summary>
        /// Handles the <see cref="Departed"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorDeparted(object sender, ElevatorMovementEventArgs args)
        {
            LogEvent("Elevator Departed", (this.CurrentFloor, args.DestinationFloor));

        }

        /// <summary>
        /// Handles the <see cref="Arriving"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorArriving(object sender, ElevatorMovementEventArgs args)
        {
            LogEvent("Elevator Arriving", ("Floor", args));

        }

        private readonly SoundPlayer soundPlayer = new SoundPlayer { Stream = Resources.elevatorDing };


        private bool TryRemoveFloor(int destination, Direction direction)
        {

            // this._floorsLock.EnterUpgradeableReadLock();
            try
            {
                var toRemove = (destination, direction);

                if (this._floorsToStopAt.Contains(toRemove))
                {
                    //       this._floorsLock.EnterWriteLock();
                    try
                    {
                        return _floorsToStopAt.TryRemove(toRemove);
                    }
                    finally
                    {
                        //         _floorsLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                //_floorsLock.ExitUpgradeableReadLock();
            }

            return false;
        }
        

        /// <summary>
        /// Handles the <see cref="Arrived"/> event. Logs a message, and removes the passengers that are getting off at the given floor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">The floor that the elevator is arriving at</param>
        private async void ElevatorArrived(object sender, ElevatorMovementEventArgs args)
        {
            try
            {
                this.CurrentFloor = args.DestinationFloor;

                this.TryRemoveFloor(args.DestinationFloor, args.Direction);

                LogEvent("Elevator Arrived", ("Floor", args.DestinationFloor));

                //_passengerLock.EnterUpgradeableReadLock();
                try
                {

                    IEnumerable<Passenger> leaving =
                        this.Passengers.Where(p => p.Path.destination == args.DestinationFloor);

                    await this.Door.WaitForDoorToOpen().ConfigureAwait(false);

                    foreach (Passenger passenger in leaving)
                    {
                        try
                        {
                            await this.RemovePassenger(passenger);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                }
                finally
                {
                    // _passengerLock.ExitUpgradeableReadLock();
                }

                // Check to see if the elevator still needs to move and everyones gone (ie waiting on another floor). 
                // If it DOES, then dispatch the elevator to the nearest destination
                if (!this.Moving)
                {
                    if (this.FloorRequests.Any() && !this.Passengers.Any())
                    {
                        //Reset the direction and then get the next floor to pick up a passenger at
                        this.RequestedDirection = this.assignDirection(FloorRequests.Min(a => Math.Abs(a.Floor - this.CurrentFloor)));
                        await this.Move().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {

            }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a value indicating whether the number of passengers is being changed
        /// </summary>
        /// <value>
        ///   <c>true</c> if updating passengers; otherwise, <c>false</c>.
        /// </value>
        public bool UpdatingPassengers { get; private set; }

        /// <summary>
        /// Adds a new <see cref="Passenger"/> to the <see cref="Elevator"/>
        /// </summary>
        /// <param name="passenger">The <see cref="Passenger"/> that will be boarding.</param>
        public async Task AddPassenger(Passenger passenger)
        {
            //await semaphore.WaitAsync().ConfigureAwait(true);
            try
            {
                UpdatingPassengers = true;
                LogEvent("Adding Passenger to Elevator");
                passenger.State = PassengerState.Transition;

                // Simulate time it takes to enter the elevator
                Thread.Sleep(Passenger.TransitionSpeed);//.ConfigureAwait(true);

                passenger.State = PassengerState.In;

                // _passengerLock.EnterWriteLock();

                try
                {
                    this._passengers.AddDistinct(passenger);
                }
                finally
                {
                    //   _passengerLock.ExitWriteLock();
                }

                PassengerAdded?.Invoke(this, passenger);

                this.OnNext(passenger.Call);

            }
            finally
            {
                UpdatingPassengers = false;
            }
        }

        /// <summary>
        /// Moves a <see cref="Passenger"/> off of the <see cref="Elevator"/>
        /// </summary>
        /// <param name="passenger">The <see cref="Passenger"/> that will be departing.</param>
        private async Task RemovePassenger(Passenger passenger)
        {
            try
            {
                UpdatingPassengers = true;
                LogEvent("Passenger Leaving Elevator");
                passenger.State = PassengerState.Transition;

                Thread.Sleep(Passenger.TransitionSpeed);//.ConfigureAwait(false);

                // _passengerLock.EnterWriteLock();

                try
                {
                    this._passengers.Remove(passenger);
                    passenger.State = PassengerState.Out;
                }
                finally
                {
                    //   _passengerLock.ExitWriteLock();
                }

                PassengerExited?.Invoke(this, passenger);

                Stats.Instance.PassengerWaitTimes.Add(passenger.TimeSpentInElevator + passenger.TimeWaiting);
            }
            finally
            {
                UpdatingPassengers = false;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Wires up this <see cref="T:ElevatorApp.Models.Elevator" /> to the given <see cref="T:ElevatorApp.Models.ElevatorMasterController" />
        /// </summary>
        public async Task Subscribe(ElevatorMasterController controller)
        {
            if (Subscribed)
                return;

            Logger.LogEvent("Subcribing elevator to MasterController", ("Elevator Number", this.ElevatorNumber.ToString()));

            this._getFloorNum = () => controller.FloorCount;

            this.Door.Opening += (a, b) =>
            {
                soundPlayer.Play();
            };

            await this.Door.Subscribe(this).ConfigureAwait(false);
            this.Subscribed = true;
        }

        private Direction assignDirection(int destination)
        {
            if (this.CurrentFloor == destination)
                return Direction.None;

            if (this.CurrentFloor < destination)
                return Direction.Up;

            return Direction.Down;
        }

        private bool _moving = false;

        private bool Moving
        {
            get => _moving;
            set
            {
                lock (_locker)
                    _moving = value;
            }
        }

        private bool RequestStarted = false;
        

        #region Movement
        private async Task PerformMovement(int destination, Direction direction, Func<int, int> floorIncrementer)
        {
            try
            {
                await semaphore.WaitAsync().ConfigureAwait(false);
                Thread.Sleep(FLOOR_MOVEMENT_SPEED.ToTimeSpan());//.ConfigureAwait(false);

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
            catch (Exception ex)
            {
                this.OnError(ex);
            }
            finally
            {
                semaphore.Release();
            }

        }

        /// <summary>
        /// Handles the logic for the movement of the <see cref="Elevator"/>.
        /// <para>Loops through the <see cref="FloorRequests"/> and moves the <see cref="Elevator"/> accordingly</para>
        /// </summary>
        private async Task Move()
        {
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

            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }
            finally
            {
                this.OnCompleted();
                // semaphore.Release();
            }
        }

        /// <summary>
        /// When the elevator is about to get near a floor
        /// </summary>
        /// <param name="args"></param>
        /// <returns><see langword="true"/> if the <see cref="Elevator"/> should stop</returns>
        private bool _approachFloor(ElevatorApproachingEventArgs args)
        {
            try
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
            catch (Exception ex)
            {
                this.OnError(ex);
                return false;
            }
        }

        /// <summary>
        /// Stars moving
        /// </summary>
        /// <param name="args"></param>
        private async Task _startMovement(ElevatorMovementEventArgs args)
        {
            try
            {
                await this.Door.WaitForDoorToClose().ConfigureAwait(false);

                this.Moving = true;

                this.State = ElevatorState.Departing;
                this.Departing?.Invoke(this, args);

                await Task.Delay(ACCELERATION_DELAY.ToTimeSpan()).ConfigureAwait(false);

                this.State = ElevatorState.Departed;
                this.Departed?.Invoke(this, args);

            }
            catch (Exception ex)
            {
                this.OnError(ex);
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
                    this.State = ElevatorState.Arriving;
                    this.Arriving?.Invoke(this, args);

                    await Task.Delay(DECELERATION_DELAY.ToTimeSpan()).ConfigureAwait(false);
                }

                if (this.State == ElevatorState.Arriving)
                {
                    this.State = ElevatorState.Arrived;
                    this.Arrived?.Invoke(this, args);
                }
                else
                {
                    //Debug.Fail("Invalid state", "Must be in state of Arriving to trigger Arrived event");
                }
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Tells the elevator that a new call has been made
        /// </summary>
        /// <param name="destination"></param>
        public void OnNext((int floor, Direction direction) destination)
        {
            RequestStarted = true;
            _floorsToStopAt.AddDistinct(destination);

            if (!Moving)
            {
                // Only start the Movement thread if the elevator hasn't already been assigned a direction
                if (this.RequestedDirection == Direction.None)
                {
                    this.RequestedDirection = destination.direction;

                    try
                    {
                        // Starts the movement in a seperate thread with Task.Run
                        Task.Factory.StartNew(this.Move, TaskCreationOptions.LongRunning).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        this.OnError(ex);
                    }
                }

            }
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            Trace.TraceError(error.ToString());
        }

        /// <summary>
        /// Runs when the <see cref="Elevator"/> has completed all of the requests it has.
        /// </summary>
        public void OnCompleted()
        {
            RequestStarted = false;
            Moving = false;
            this.RequestedDirection = Direction.None;
            this.State = ElevatorState.Idle;
            Logger.LogEvent("Elevator path completed.");
        }
    }
}
