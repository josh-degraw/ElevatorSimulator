using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;
using static ElevatorApp.Util.Logger;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents an elevator
    /// </summary>
    public class Elevator : ModelBase, ISubcriber<ElevatorMasterController>
    {
        #region Backing fields

        private static int _RegisteredElevators = 0;
        private ElevatorDirection _direction = ElevatorDirection.None;
        private ElevatorState _elevatorState = ElevatorState.Idle;

        private int
            _speed = 800,
            _currentSpeed = 0,
            _totalCapacity,
            _currentFloor = 1;

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

        /// <summary>
        /// The call that is currently being processed. If the elevator is <see cref="ElevElevatorDirection.Noneand there are no <see cref="ElevatorCall"/>s in the queue, this will be <see langword="null"/>
        /// </summary>
        public ElevatorCall? CurrentCall
        {
            get
            {
                if (_path.TryPeek(out ElevatorCall call))
                    return call;

                return null;
            }
        }

        private readonly AsyncObservableCollection<ElevatorCall> _path = new AsyncObservableCollection<ElevatorCall>();
        private readonly AsyncObservableCollection<Passenger> _passengers = new AsyncObservableCollection<Passenger>();

        ///<inheritdoc/>
        public bool Subscribed { get; private set; }

        /// <summary>
        /// The <see cref="ElevatorCall"/>s that remain to be fulfilled by this <see cref="Elevator"/>.
        /// </summary>
        public IReadOnlyCollection<ElevatorCall> Path => _path;

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
        /// Called when the elevator is about to arrive
        /// </summary>
        public event EventHandler<ElevatorCall> Arriving;

        /// <summary>
        /// Called when the <see cref="Elevator"/> has arrived, right before the doors open
        /// </summary>
        public event EventHandler<int> Arrived;

        /// <summary>
        /// Called when the <see cref="Elevator"/> is about to leave, right after the doors have closed
        /// </summary>
        public event EventHandler<ElevatorCall> Departing;

        /// <summary>
        /// Called when the <see cref="Elevator"/> has just left
        /// </summary>
        public event EventHandler<ElevatorCall> Departed;

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
        public Elevator(int initialFloor=1)
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
        /// <param name="call"></param>
        private void ElevatorDeparting(object sender, ElevatorCall call)
        {
            this.State = ElevatorState.Departing;
            this.Direction = (ElevatorDirection)call.RequestDirection;
            LogEvent("Elevator Departing", ("From", call.SourceFloor), ("To", call.DestinationFloor));
        }

        /// <summary>
        /// Handles the <see cref="Departed"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="call"></param>
        private void ElevatorDeparted(object sender, ElevatorCall call)
        {
            this.State = ElevatorState.Departed;
            LogEvent("Elevator Departed", ("From", call.SourceFloor), ("To", call.DestinationFloor));
        }

        /// <summary>
        /// Handles the <see cref="Arriving"/> event. Just logs an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElevatorArriving(object sender, ElevatorCall e)
        {
            this.State = ElevatorState.Arriving;
            LogEvent("Elevator Arriving", ("At Floor", e.DestinationFloor));
        }

        /// <summary>
        /// Handles the <see cref="Arrived"/> event. Logs a message, and removes the passengers that are getting off at the given floor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="floor">The floor that the elevator is arriving at</param>
        private async void ElevatorArrived(object sender, int floor)
        {
            this.State = ElevatorState.Arrived;
            LogEvent("Elevator Arrived", ("At Floor", floor));
            IEnumerable<Passenger> leaving = this.Passengers.Where(p => p.Path.destination == floor);
            foreach (Passenger passenger in leaving)
            {
                await this.RemovePassenger(passenger);
            }
        }

        /// <summary>
        /// Triggers the <see cref="Arrived"/> event, and sets the <see cref="CurrentFloor"/>
        /// </summary>
        /// <param name="call"></param>
        private Task OnArrived(ElevatorCall call)
        {
            //Play ding sound on arrival and log it
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.Stream = Properties.Resources.elevatorDing;
            player.Play();
            Console.WriteLine("Arrived");
            // Simulate slowdown after arriving
            this.Arrived?.Invoke(this, call.DestinationFloor);

            this.CurrentFloor = call.DestinationFloor;

            return Task.CompletedTask;
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
            this._path.AddDistinct(new ElevatorCall(passenger.Path.source, passenger.Path.destination));

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

            this.Door.Closed += async (e, args) =>
            {
                if (this.Path.Count > 0)
                    await this.Move();
            };

            await Task.WhenAll(subscribeButtonPanel, subscribeDoor).ConfigureAwait(false);


            this.Subscribed = true;
        }

        /// <summary>
        /// Adds an <see cref="ElevatorCall"/> to this <see cref="Elevator"/>, and, if the <see cref="Elevator"/> is currently idle, starts up the process
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        internal async Task Dispatch(ElevatorCall call)
        {
            this._path.Enqueue(call);

            if (this.Direction == ElevatorDirection.None)
            {
                this.Direction = (ElevatorDirection)call.RequestDirection;

                // Starts the movement in a seperate thread with Task.Run
                await Task.Run(() => this.Move().ConfigureAwait(false));
            }
        }

        /// <summary>
        /// Handles the logic for the movement of the <see cref="Elevator"/>.
        /// Loops through the <see cref="Path"/> and moves the <see cref="Elevator"/> accordingly
        /// </summary>
        private async Task Move()
        {
            //Wait for the door to be closed
            while (this.Door.DoorState != DoorState.Closed)
                await Task.Delay(1);

            while (this._path.TryDequeue(out ElevatorCall call))
            {
                this.Direction = this.CurrentFloor < call.DestinationFloor
                                 ? ElevatorDirection.GoingUp
                                 : ElevatorDirection.GoingDown;

                this.Departing?.Invoke(this, call);
                await Task.Delay(ACCELERATION_DELAY.ToTimeSpan());

                this.Departed?.Invoke(this, call);

                for (int i = call.SourceFloor; i < call.DestinationFloor; i++)
                {
                    await Task.Delay(FLOOR_MOVEMENT_SPEED.ToTimeSpan());
                    LogEvent($"Elevator passing floor {i}");
                }

                // Logic
                this.Arriving?.Invoke(this, call);
                await Task.Delay(DECELERATION_DELAY.ToTimeSpan());
                await this.OnArrived(call).ConfigureAwait(false);
            }

            this.Direction = ElevatorDirection.None;
            this.State = ElevatorState.Idle;
        }
        #endregion

    }
}
