using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Properties;
using ElevatorApp.Util;
using MoreLinq;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using static ElevatorApp.Util.Logger;

namespace ElevatorApp.Models
{
    /// <inheritdoc cref="ISubcriber{T}"/>
    /// <summary>
    /// Represents an elevator.
    /// <para>Implements <see cref="T:System.IObserver`1"/> to observe when a floor wants an elevator</para>
    /// </summary>
    public class Elevator : ModelBase, ISubcriber<ElevatorMasterController>, IObserver<ElevatorCall>
    {
        #region Private Fields

        /// <summary>
        /// The registered elevators.
        /// </summary>
        [Obsolete("No longer needed because we're only using one Elevator now")]
        private static int _RegisteredElevators = 0;

        /// <summary>
        /// The floors to stop at
        /// </summary>
        private readonly AsyncObservableCollection<ElevatorCall> _floorsToStopAt = new AsyncObservableCollection<ElevatorCall>();

        /// <summary>
        /// Used to prevent threading issues
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// The passengers inside the elevator
        /// </summary>
        private readonly AsyncObservableCollection<Passenger> _passengers = new AsyncObservableCollection<Passenger>();

        /// <summary>
        /// Used to prevent threading issues, has async support
        /// </summary>
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private Direction _direction = Direction.None;

        private ElevatorState _elevatorState = ElevatorState.Idle;

        /// <summary>
        /// Get the number of floors
        /// </summary>
        private Func<int> _getFloorNum = () => 4;

        private bool _moving = false;

        private int
                    _speed = 800,
                    _currentSpeed = 0,
                    _totalCapacity,
                    _currentFloor = 1,
                    _nextFloor = 1;

        #endregion Private Fields

        #region Private Properties

        /// <summary>
        /// Represents whether the elevator is currently moving
        /// </summary>
        private bool Moving
        {
            get => this._moving;

            set
            {
                lock (this._locker) this._moving = value;
            }
        }

        /// <summary>
        /// Used to make a `ding` sound when the elevator arrives
        /// </summary>
        private SoundPlayer soundPlayer { get; } = new SoundPlayer { Stream = Resources.elevatorDing };

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Approaches the floor.
        /// <para>
        /// If during the course of the events, it is determined that the elevator should stop at the intermediate floor,
        /// this method returns true to indicate that the elevator should stop.
        /// </para>
        /// </summary>
        /// <param name="args">The <see cref="ElevatorApproachingEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        /// <returns><see langword="true"/> if the <see cref="Elevator"/> should stop</returns>
        private bool _approachFloor(ElevatorApproachingEventArgs args)
        {
            try
            {
                LogEvent("Elevator approaching ", ("Floor", args.IntermediateFloor), ("Eventual Destination", args.DestinationFloor));

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

        /// <summary>
        /// Stars moving
        /// </summary>
        /// <param name="args"></param>
        private async Task _startMovement(ElevatorMovementEventArgs args)
        {
            try
            {
                // If it's trying to go to the floor it's already on, that's just a bucket of nonsense
                if (args.DestinationFloor == this.CurrentFloor)
                    return;

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
        /// Validates the state change.
        /// </summary>
        /// <param name="prev">The previous.</param>
        /// <param name="next">The next.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">prev - Unkown state</exception>
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
                    throw new ArgumentOutOfRangeException(nameof(prev), prev, "Unkown state");
            }
        }

        /// <summary>
        /// Returns the direction the elevator should move, assuming the given destination floor. Does not modify the
        /// direction the <see cref="Elevator"/> is going in.
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        [Pure]
        private Direction assignDirection(int destination)
        {
            if (this.CurrentFloor == destination)
                return Direction.None;

            if (this.CurrentFloor < destination)
                return Direction.Up;

            return Direction.Down;
        }

        /// <summary>
        /// Handles the <see cref="Arrived"/> event. Logs a message, and removes the passengers that are getting off at
        /// the given floor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">The floor that the elevator is arriving at</param>
        private async void ElevatorArrived(object sender, ElevatorMovementEventArgs args)
        {
            try
            {
                this.CurrentFloor = args.DestinationFloor;

                this.TryRemoveCall(args.Call);

                ElevatorCall calledFromFloor = this._floorsToStopAt.FirstOrDefault(a => a.Floor == this.CurrentFloor && !a.FromPassenger);

                if (calledFromFloor != null) this.TryRemoveCall(calledFromFloor);

                LogEvent("Elevator Arrived", ("Floor", args.DestinationFloor));

                IEnumerable<Passenger> leaving =
                        this.Passengers.Where(p => p.Path.destination == args.DestinationFloor);

                await this.Door.WaitForDoorToOpen().ConfigureAwait(false);

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

                // Check to see if the elevator still needs to move and everyones gone (ie waiting on another floor). If
                // it DOES, then dispatch the elevator to the nearest destination
                if (!this.Moving)
                {
                    if (this.FloorRequests.Any() && !this.Passengers.Any())
                    {
                        //Reset the direction and then get the next floor to pick up a passenger at
                        this.RequestedDirection = this.assignDirection(this.FloorRequests.Min(a => Math.Abs(a.Floor - this.CurrentFloor)));
                        await this.Move().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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
        /// Handles the <see cref="Departing"/> event. Sets the state of the elevator to the direction it will be moving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorDeparting(object sender, ElevatorMovementEventArgs args)
        {
            LogEvent("Elevator Departing", (this.CurrentFloor, args.DestinationFloor));
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

                    var (destination, direction) = this.FloorRequests
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

                    this.RequestedDirection = this.assignDirection(destination);

                    if (this.RequestedDirection == Direction.None)
                        break;

                    switch (this.RequestedDirection)
                    {
                        case Direction.Up:

                            this.NextFloor = this.CurrentFloor + 1;
                            await this._startMovement(new ElevatorMovementEventArgs(destination, direction));

                            do
                            {
                                this.NextFloor = this.CurrentFloor + 1;
                                await this.PerformMovement(destination, direction, a => a + 1);
                            }
                            while (this.NextFloor < destination);

                            break;

                        case Direction.Down:
                            this.NextFloor = this.CurrentFloor - 1;
                            await this._startMovement(new ElevatorMovementEventArgs(destination, direction));

                            do
                            {
                                this.NextFloor = this.CurrentFloor - 1;
                                await this.PerformMovement(destination, direction, a => a - 1);
                            }
                            while (this.NextFloor > destination);

                            break;
                    }

                    await this._arriveAtFloor(new ElevatorMovementEventArgs(destination, direction));
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

        private async Task PerformMovement(int destination, Direction direction, Func<int, int> floorIncrementer)
        {
            try
            {
                await this._semaphore.WaitAsync().ConfigureAwait(false);
                Thread.Sleep(FLOOR_MOVEMENT_SPEED.ToTimeSpan());//.ConfigureAwait(false);

                var args = new ElevatorApproachingEventArgs(this.NextFloor, destination, direction);

                // If the elevator should stop at this floor
                bool shouldStop = this._approachFloor(args);

                if (shouldStop)
                {
                    var call = new ElevatorMovementEventArgs(this.NextFloor, direction);
                    await this._arriveAtFloor(call);

                    this.NextFloor = floorIncrementer(this.CurrentFloor);

                    await this._startMovement(call);

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
                this._semaphore.Release();
            }
        }

        /// <summary>
        /// Moves a <see cref="Passenger"/> off of the <see cref="Elevator"/>
        /// </summary>
        /// <param name="passenger">The <see cref="Passenger"/> that will be departing.</param>
        private Task RemovePassenger(Passenger passenger)
        {
            try
            {
                this.UpdatingPassengers = true;
                LogEvent("Passenger Leaving Elevator");
                passenger.State = PassengerState.Transition;

                Thread.Sleep(Passenger.TransitionSpeed);

                this._passengers.Remove(passenger);
                passenger.State = PassengerState.Out;

                this.PassengerExited?.Invoke(this, passenger);

                Stats.Instance.PassengerWaitTimes.Add(passenger.TimeSpentInElevator + passenger.TimeWaiting);
            }
            finally
            {
                this.UpdatingPassengers = false;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Tries to remove the given call from the request queue.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns><c>true</c> if successfully removed; otherwise false</returns>
        private bool TryRemoveCall(ElevatorCall toRemove)
        {
            if (this._floorsToStopAt.Contains(toRemove))
            {
                return this._floorsToStopAt.TryRemove(toRemove);
            }

            return false;
        }

        #endregion Private Methods

        #region Public Fields

        /// <summary>
        /// The time it takes to get up to full speed once departing
        /// </summary>
        public static readonly Duration ACCELERATION_DELAY = Duration.FromSeconds(4);

        /// <summary>
        /// The time it takes to slow down before arriving
        /// </summary>
        public static readonly Duration DECELERATION_DELAY = ACCELERATION_DELAY * 2;

        /// <summary>
        /// The time it takes to move from one floor to another
        /// </summary>
        public static readonly Duration FLOOR_MOVEMENT_SPEED = Duration.FromSeconds(7);

        #endregion Public Fields

        #region Public Events

        /// <summary>
        /// Called when the <see cref="Elevator"/> is getting close to a <see cref="Floor"/>, but before it's officially
        /// started arriving. After this event is handled,
        /// </summary>
        public event EventHandler<ElevatorApproachingEventArgs> Approaching;

        /// <summary>
        /// Called when the <see cref="Elevator"/> has arrived, right before the doors open
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Arrived;

        /// <summary>
        /// Called when the elevator is about to arrive
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Arriving;

        /// <summary>
        /// Called when the <see cref="Elevator"/> has just left
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Departed;

        /// <summary>
        /// Called when the <see cref="Elevator"/> is about to leave, right after the doors have closed
        /// </summary>
        public event EventHandler<ElevatorMovementEventArgs> Departing;

        /// <summary>
        /// Called when a <see cref="Passenger"/> has just entered the <see cref="Elevator"/>
        /// </summary>
        public event EventHandler<Passenger> PassengerAdded;

        /// <summary>
        /// Called when a <see cref="Passenger"/> has just left the <see cref="Elevator"/>
        /// </summary>
        public event EventHandler<Passenger> PassengerExited;

        #endregion Public Events

        #region Public Constructors

        /// <summary>
        /// Instantiates a new <see cref="Elevator"/>
        /// </summary>
        /// <param name="initialFloor">The floor to start on</param>
        public Elevator(int initialFloor = 1)
        {
            this._currentFloor = initialFloor;
            //this.ElevatorNumber = ++_RegisteredElevators;

            LogEvent("Initializing Elevator", ("ElevatorNumber", this.ElevatorNumber));

            this._floorsToStopAt.CollectionChanged += (sender, args) =>
            {
                try
                {
                    LogEvent("Elevator Queue changed", ("Path", this._floorsToStopAt.OrderBy(a => a.Floor).ToDelimitedString(",")));
                    this.OnPropertyChanged(this.FloorsToStopAtString, nameof(this.FloorsToStopAtString));
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            };

            this.Arriving += this.ElevatorArriving;
            this.Arrived += this.ElevatorArrived;

            this.Departing -= this.ElevatorDeparting;
            this.Departed -= this.ElevatorDeparted;

            this.Departing += this.ElevatorDeparting;
            this.Departed += this.ElevatorDeparted;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Same as <see cref="FLOOR_MOVEMENT_SPEED"/>, but in a format that can be used by the GUI
        /// </summary>
        public static System.Windows.Duration FloorMovementSpeed => new System.Windows.Duration(FLOOR_MOVEMENT_SPEED.ToTimeSpan());

        /// <summary>
        /// The button panel inside of the elevator
        /// </summary>
        [Obsolete("For purposes of the program, this is being handled by the floors", true)]
        public ButtonPanel ButtonPanel { get; }

        /// <summary>
        /// Represents the total weight currently held on the <see cref="Elevator"/>
        /// </summary>
        public int CurrentCapacity => this.Passengers?.Count ?? 0;

        /// <summary>
        /// Represents the current floor of the <see cref="Elevator"/>
        /// <para></para>
        /// </summary>
        public int CurrentFloor
        {
            get => this._currentFloor;
            private set => this.SetProperty(ref this._currentFloor, value.KeepInRange(0, this._getFloorNum())); //TODO: remove hardcoded floor number
        }

        /// <summary>
        /// TODO
        /// </summary>
        public int CurrentSpeed
        {
            get => this._currentSpeed;
            private set => this.SetProperty(ref this._currentSpeed, value);
        }

        /// <summary>
        /// The direction the elevator is `actually` going.
        /// <para>
        /// i.e. if the elevator is on floor 1, and someone on floor 3 requests to go down, once moving,
        /// <see cref="RequestedDirection"/> will be <see cref="Enums. Direction.Down"/> while <see cref="Direction"/>
        /// will be <see cref="Enums.Direction.Up"/>,
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

        /// <summary>
        /// The door of the <see cref="Elevator"/>
        /// </summary>
        public Door Door { get; } = new Door();

        /// <summary>
        /// The number of this <see cref="Elevator"/>. This is generated in order of initialization
        /// </summary>
        public int ElevatorNumber { get; }

        /// <summary>
        /// A collection of the floor numbers that want the <see cref="Elevator"/> to stop there
        /// </summary>
        public IReadOnlyCollection<ElevatorCall> FloorRequests => this._floorsToStopAt;

        /// <summary>
        /// A string representation of the floors that are currently in the queue
        /// </summary>
        public string FloorsToStopAtString => this.FloorRequests.ToDelimitedString(", ");

        /// <summary>
        /// Represents the closest elevator, If the <see cref="Elevator"/> is idle, this will be the same as <see cref="CurrentFloor"/>
        /// </summary>
        public int NextFloor
        {
            get => this._nextFloor;
            private set => this.SetProperty(ref this._nextFloor, value.KeepInRange(0, this._getFloorNum()));
        }

        /// <summary>
        /// The people currently inside the <see cref="Elevator"/>
        /// </summary>
        public IReadOnlyCollection<Passenger> Passengers => this._passengers;

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
        /// The direction the elevator has been requested to go once it arrives
        /// <para>
        /// i.e. if the elevator is on floor 1, and someone on floor 3 requests to go down, once moving,
        /// <see cref="RequestedDirection"/> will be <see cref="Enums. Direction.Down"/> while <see cref="Direction"/>
        /// will be <see cref="Enums.Direction.Up"/>,
        /// </para>
        /// </summary>
        public Direction RequestedDirection
        {
            get => this._direction;
            private set => this.SetProperty(ref this._direction, value);
        }

        /// <summary>
        /// How fast the elevator is going
        /// <para>TODO: Set the speed when the elevator moves</para>
        /// </summary>
        public int Speed
        {
            get => this._speed;

            private set
            {
                this.SetProperty(ref this._speed, value);
                this.OnPropertyChanged(this.RelativeSpeed, nameof(this.RelativeSpeed));
            }
        }

        //{
        //    get => _direction;
        //    private set => SetProperty(ref _direction, value);
        //}
        /// <summary>
        /// Represents the state of the elevator
        /// </summary>
        public ElevatorState State
        {
            get => this._elevatorState;

            private set
            {
                Debug.Assert(this._validateStateChange(this._elevatorState, value), "Invalid state change for elevator", "{0} -> {1}", this._elevatorState, value);
                this.SetProperty(ref this._elevatorState, value);
            }
        }

        /// <summary>
        /// Represents whether or not this object has performed the necessary steps to subscribe to the source.
        /// </summary>
        /// <inheritdoc/>
        public bool Subscribed { get; private set; }

        /// <summary>
        /// The total number of <see cref="Passenger"/> s the <see cref="Elevator"/> can hold
        /// </summary>
        public int TotalCapacity
        {
            get => this._totalCapacity;
            set => this.SetProperty(ref this._totalCapacity, value);
        }

        /// <summary>
        /// Gets a value indicating whether the number of passengers is being changed
        /// </summary>
        /// <value><c>true</c> if updating passengers; otherwise, <c>false</c>.</value>
        public bool UpdatingPassengers { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Adds a new <see cref="Passenger"/> to the <see cref="Elevator"/>
        /// </summary>
        /// <param name="passenger">The <see cref="Passenger"/> that will be boarding.</param>
        public Task AddPassenger(Passenger passenger)
        {
            //await semaphore.WaitAsync().ConfigureAwait(true);
            try
            {
                this.UpdatingPassengers = true;
                LogEvent("Adding Passenger to Elevator");
                passenger.State = PassengerState.Transition;

                // Simulate time it takes to enter the elevator
                Thread.Sleep(Passenger.TransitionSpeed);//.ConfigureAwait(true);

                passenger.State = PassengerState.In;

                this._passengers.AddDistinct(passenger);

                this.PassengerAdded?.Invoke(this, passenger);

                this.OnNext(passenger.Call);
            }
            finally
            {
                this.UpdatingPassengers = false;
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Runs when the <see cref="T:ElevatorApp.Models.Elevator"/> has completed all of the requests it has.
        /// </summary>
        public void OnCompleted()
        {
            this.Moving = false;
            this.RequestedDirection = Direction.None;
            this.State = ElevatorState.Idle;
            LogEvent("Elevator path completed.");
        }

        /// <inheritdoc/>
        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            Trace.TraceError(error.ToString());
        }

        /// <inheritdoc/>
        /// <summary>
        /// Tells the elevator that a new call has been made
        /// </summary>
        /// <param name="destination"></param>
        public void OnNext(ElevatorCall destination)
        {
            Task.Run(() =>
            {
                while (this.Door.IsOpenedOrOpening && destination.Floor != this.CurrentFloor)
                {
                    Thread.Sleep(500); //.ConfigureAwait(false);
                }

                if (!destination.FromPassenger && destination.Floor == this.CurrentFloor &&
                    (this.State == ElevatorState.Arrived || this.State == ElevatorState.Idle))
                {
                    // Wait for door to open here so that the passengers who started opening the door get in first
                    this.Door.WaitForDoorToOpen().ConfigureAwait(false).GetAwaiter().GetResult();
                    Thread.Sleep(500); //.ConfigureAwait(false);
                }

                this._floorsToStopAt.AddDistinct(destination);


                if (this.Moving || this.RequestedDirection != Direction.None) return;
                // Only start the Movement thread if the elevator hasn't already been assigned a direction

                this.RequestedDirection = destination.Direction;

                try
                {
                    // Starts the movement in a seperate thread with Task.Run Not awaited because it's running
                    // somewhere else and we don't care about the results here
                    while (this.Door.IsOpenedOrOpening)
                    {
                        Thread.Sleep(500); //.ConfigureAwait(false);
                    }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Factory.StartNew(this.Move, TaskCreationOptions.LongRunning).ConfigureAwait(false);
#pragma warning restore CS4014
                }
                catch (Exception ex)
                {
                    this.OnError(ex);
                }
            });
        }

        /// <inheritdoc/>
        /// <summary>
        /// Wires up this <see cref="T:ElevatorApp.Models.Elevator"/> to the given <see cref="T:ElevatorApp.Models.ElevatorMasterController"/>
        /// </summary>
        public async Task Subscribe(ElevatorMasterController controller)
        {
            if (this.Subscribed)
                return;

            LogEvent("Subcribing elevator to MasterController", ("Elevator Number", this.ElevatorNumber.ToString()));

            this._getFloorNum = () => controller.FloorCount;

            this.Door.Opening += (a, b) =>
            {
                this.soundPlayer.Play();
            };

            await this.Door.Subscribe(this).ConfigureAwait(false);
            this.Subscribed = true;
        }

        #endregion Public Methods
    }
}