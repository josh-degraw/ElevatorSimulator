using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;
using MoreLinq;
using static ElevatorApp.Util.Logger;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents an elevator.
    /// <para>Implements <see cref="IObserver{T}"/> to observe when a floor wants an elevator</para>
    /// </summary>
    public class Elevator : ModelBase, ISubcriber<ElevatorMasterController>
    {

        private IDisposable _unsubscriber;
        #region Backing fields

        private static int _RegisteredElevators = 0;
        private ElevatorDirection _direction = ElevatorDirection.None;
        private ElevatorState _elevatorState = ElevatorState.Idle;

        private int
            _speed = 800,
            _currentSpeed = 0,
            _totalCapacity,
            _currentFloor = 1;

        //[Obsolete]
        //private readonly AsyncObservableCollection<ElevatorCall> _path = new AsyncObservableCollection<ElevatorCall>();
        private readonly AsyncObservableCollection<Passenger> _passengers = new AsyncObservableCollection<Passenger>();
        private readonly AsyncObservableCollection<int> _floorsToStopAt = new AsyncObservableCollection<int>();

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
            set
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
                    case ElevatorDirection.None: return 0;
                    case ElevatorDirection.GoingUp: return this.Speed;
                    case ElevatorDirection.GoingDown: return -this.Speed;

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
            set => SetProperty(ref _currentSpeed, value);
        }

        /// <summary>
        /// Represents the total weight currently held on the <see cref="Elevator"/>
        /// </summary>
        public int CurrentCapacity => Passengers?.Count ?? 0;

        /// <summary>
        /// The direction the elevator is going
        /// </summary>
        public ElevatorDirection Direction
        {
            get => _direction;
            set => SetProperty(ref _direction, value);
        }

        /// <summary>
        /// Represents the state of the elevator
        /// </summary>
        public ElevatorState State
        {
            get => _elevatorState;
            set => SetProperty(ref _elevatorState, value);
        }

        /// <summary>
        /// Represents the current floor of the <see cref="Elevator"/>
        /// <para>
        /// TODO: Either switch this to int? and have <see langword="null"/> mean it's moving, or figure out a better way to show the floor it's passing while it's moving
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
                    case ElevatorDirection.None: return this.CurrentFloor;
                    case ElevatorDirection.GoingUp: return this.CurrentFloor + 1;
                    case ElevatorDirection.GoingDown: return this.CurrentFloor - 1;

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
        public IReadOnlyCollection<int> FloorsToStopAt => _floorsToStopAt;

        /// <summary>
        /// The people currently inside the <see cref="Elevator"/>
        /// </summary>
        public IReadOnlyCollection<Passenger> Passengers => _passengers;

        /// <summary>
        /// The button panel inside of the elevator
        /// </summary>
        [Obsolete("For purposes of the program, this is being handled by the floors")]
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
            this.ButtonPanel = new ButtonPanel();
            this.Arriving += ElevatorArriving;
            this.Arrived += ElevatorArrived;

            this.Departing += ElevatorDeparting;
            this.Departed += ElevatorDeparted;
        }

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
            this.State = ElevatorState.Departing;


            if (this.CurrentFloor < args.DestinationFloor)
            {
                this.Direction = ElevatorDirection.GoingUp;
            }
            else if (this.CurrentFloor > args.DestinationFloor)
            {
                this.Direction = ElevatorDirection.GoingDown;
            }
            else
            {
                this.Direction = ElevatorDirection.None;
            }

            LogEvent("Elevator Departing", ("From", this.CurrentFloor), ("To", args.DestinationFloor));
        }

        /// <summary>
        /// Handles the <see cref="Departed"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorDeparted(object sender, ElevatorMovementEventArgs args)
        {
            this.State = ElevatorState.Departed;
            LogEvent("Elevator Departed", ("From", this.CurrentFloor), ("To", args));
        }

        /// <summary>
        /// Handles the <see cref="Arriving"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ElevatorArriving(object sender, ElevatorMovementEventArgs args)
        {
            this.State = ElevatorState.Arriving;
            LogEvent("Elevator Arriving", ("At Floor", args));
        }

        /// <summary>
        /// Handles the <see cref="Arrived"/> event. Logs a message, and removes the passengers that are getting off at the given floor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">The floor that the elevator is arriving at</param>
        private void ElevatorArrived(object sender, ElevatorMovementEventArgs args)
        {
            try
            {
                this.State = ElevatorState.Arrived;
                LogEvent("Elevator Arrived", ("At Floor", args.DestinationFloor));

                IEnumerable<Passenger> leaving = this.Passengers.Where(p => p.Path.destination == args.DestinationFloor);

                foreach (Passenger passenger in leaving)
                {
                    try
                    {
                        this.RemovePassenger(passenger).GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
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
            this._floorsToStopAt.AddDistinct(passenger.Path.destination);

            PassengerAdded?.Invoke(this, passenger);
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
            Task subscribeButtonPanel = this.ButtonPanel.Subscribe((controller, this));
            Task subscribeDoor = this.Door.Subscribe(this);

            //this.Door.Closed += async (e, args) =>
            //{
            //    if (this.FloorsToStopAt.Count > 0)
            //        await this.Move();
            //};

            await Task.WhenAll(subscribeButtonPanel, subscribeDoor).ConfigureAwait(false);
            this.Subscribed = true;
        }

        private ElevatorDirection assignDirection(int destination)
        {
            if (this.CurrentFloor == destination)
                return ElevatorDirection.None;

            else if (this.CurrentFloor < destination)
                return ElevatorDirection.GoingUp;

            else
                return ElevatorDirection.GoingDown;
        }

        /// <summary>
        /// Adds a floor number to this <see cref="Elevator"/>, and, if the <see cref="Elevator"/> is currently idle, starts up the process.
        /// <para>
        /// If the <see cref="Elevator"/> is already at the <see cref="Floor"/> that has been requested, trigger the <see cref="Arrived"/> event and not process any further
        /// </para>
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        internal async Task Dispatch(int destination)
        {
            // If the elevator is already at the floor that has been requested, trigger the Arrived event and stop worrying about stuff for now.
            if (this.CurrentFloor == destination)
            {
                this.Arrived?.Invoke(this, new ElevatorMovementEventArgs(destination, Enums.Direction.None));
                return;
            }

            if (!_floorsToStopAt.Contains(destination))
                _floorsToStopAt.AddDistinct(destination);

            if (this.Direction == ElevatorDirection.None || (!_moving && this.State == ElevatorState.Arrived))
            {
                // Starts the movement in a seperate thread with Task.Run
                await Task.Run(this.Move).ConfigureAwait(false);
            }
        }

        private bool _moving = false;

        /// <summary>
        /// Handles the logic for the movement of the <see cref="Elevator"/>.
        /// Loops through the <see cref="FloorsToStopAt"/> and moves the <see cref="Elevator"/> accordingly
        /// </summary>
        private async Task Move()
        {
            try
            {
                //Wait for the door to be closed
                while (this.Door.DoorState != DoorState.Closed)
                    await Task.Delay(1);

                // Get the closest floor to the current
                if (NextFloor == this.CurrentFloor)
                {
                    // If this is here, the elevator is not moving.
                    // If so, if there are any floors requested, go to the nearest one
                }

                while (this._floorsToStopAt.Any())
                {
                    _moving = true;
                    int destination = FloorsToStopAt.MinBy(a => Math.Abs(a - this.CurrentFloor));
                    this.Direction = assignDirection(destination);

                    var call = new ElevatorMovementEventArgs(destination, (Direction)this.Direction);

                    await _startMovement(call);

                    for (int i = this.CurrentFloor; i < destination; i++)
                    {
                        await Task.Delay(FLOOR_MOVEMENT_SPEED.ToTimeSpan());
                        int nextFloor = i + 1;

                        var args = new ElevatorApproachingEventArgs(nextFloor, destination, call.Direction);

                        // If the elevator should stop at this floor
                        if (_approachFloor(args))
                        {
                            LogEvent($"Elevator approaching floor {nextFloor}");
                            await _arriveAtFloor(nextFloor, args);

                            // Start moving to the next floor
                            call = new ElevatorMovementEventArgs(++i, call.Direction);
                            await _startMovement(call);
                        }
                        else if (nextFloor != destination)
                        {
                            this.CurrentFloor = nextFloor; // Set the current floor
                        }
                    }

                    await _arriveAtFloor(destination, call);
                    _floorsToStopAt.Remove(destination);
                }
                _moving = false;
                this.Direction = ElevatorDirection.None;
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
            this.Approaching?.Invoke(this, args);

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
                this.State = ElevatorState.Departing;
                this.Departing?.Invoke(this, args);
                await Task.Delay(ACCELERATION_DELAY.ToTimeSpan());

                this.State = ElevatorState.Departed;
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
        /// <param name="nextDestination"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task _arriveAtFloor(int nextDestination, ElevatorMovementEventArgs args)
        {
            try
            {
                this.State = ElevatorState.Arriving;
                this.Arriving?.Invoke(this, args);
                await Task.Delay(DECELERATION_DELAY.ToTimeSpan());

                this.CurrentFloor = nextDestination;

                this.State = ElevatorState.Arrived;
                this.Arrived?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion

    }
}
