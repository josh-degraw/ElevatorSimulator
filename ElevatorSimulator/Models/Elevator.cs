using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;
using static ElevatorApp.Util.Logger;

namespace ElevatorApp.Models
{
    public class Elevator : ModelBase, ISubcriber<ElevatorMasterController>
    {
        private static int _RegisteredElevators = 0;

        #region Backing fields

        private int _speed, _capacity, _currentFloor = 1;
        private ElevatorState _state;

        #endregion

        #region INotifyParent Properties

        public int Capacity
        {
            get => _capacity;
            set => SetProperty(ref _capacity, value);
        }

        public int Speed
        {
            get => _speed;
            set => SetProperty(ref _speed, value);
        }

        public int CurrentWeight => Passengers?.Count ?? 0;

        public ElevatorState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public int CurrentFloor
        {
            get => _currentFloor;
            private set => SetProperty(ref _currentFloor, value);
        }

        #endregion

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


        public IReadOnlyCollection<ElevatorCall> Path => _path;
        public IReadOnlyCollection<Passenger> Passengers => _passengers;

        public ButtonPanel ButtonPanel { get; }

        public Door Door { get; } = new Door();

        public event EventHandler<ElevatorCall> Arriving;
        public event EventHandler<ElevatorCall> Arrived;
        public event EventHandler<ElevatorCall> Departing;
        public event EventHandler<ElevatorCall> Departed;

        public event EventHandler<Passenger> PassengerAdded = (_, p) => LogEvent("Passenger Added to Elevator", parameters: p);
        public event EventHandler<Passenger> PassengerExited = (_, p) => LogEvent("Passenger Exited Elevator", parameters: p);


        private Task OnArrived(ElevatorCall call)
        {
            Console.WriteLine("Arrived");
            // Simulate slowdown after arriving
            this.Arrived?.Invoke(this, call);

            this.CurrentFloor = call.DestinationFloor;

            return Task.CompletedTask;
        }

        internal async Task Dispatch(ElevatorCall call)
        {
            this._path.Enqueue(call);

            if (this.State == ElevatorState.Idle)
            {
                this.State = (ElevatorState)call.RequestDirection;
                await Task.Run(() => this.Move().ConfigureAwait(false));
            }
        }

        public static readonly Duration FLOOR_MOVEMENT_SPEED = Duration.FromSeconds(7);

        public static readonly Duration ACCELERATION_DELAY = Duration.FromSeconds(4);
        public static readonly Duration DECELERATION_DELAY = ACCELERATION_DELAY * 2;

        public static System.Windows.Duration FloorMovementSpeed => new System.Windows.Duration(FLOOR_MOVEMENT_SPEED.ToTimeSpan());

        private async Task Move()
        {
            //Wait for the door to be closed
            while (this.Door.DoorState != DoorState.Closed)
                await Task.Delay(2000);


            while (this._path.TryDequeue(out ElevatorCall call))
            {
                this.State = this.CurrentFloor < call.DestinationFloor
                                 ? ElevatorState.GoingUp
                                 : ElevatorState.GoingDown;

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

            this.State = ElevatorState.Idle;
        }

        public int ElevatorNumber { get; }

        public Elevator()
        {
            this.ElevatorNumber = ++_RegisteredElevators;

            Logger.LogEvent("Initializing Elevator", ("ElevatorNumber", this.ElevatorNumber));
            this.ButtonPanel = new ButtonPanel();
            this.Arriving += ElevatorArriving;
            this.Arrived += ElevatorArrived;

            this.Departing += ElevatorDeparting;
            this.Departed += ElevatorDeparted;
        }

        public Elevator(int initialFloor) : this()
        {
            this.CurrentFloor = initialFloor;
        }

        private void ElevatorDeparting(object sender, ElevatorCall call)
        {
            LogEvent("Elevator Departing", ("From", call.SourceFloor), ("To", call.DestinationFloor));
            this.State = (ElevatorState)call.RequestDirection;
        }

        private void ElevatorDeparted(object sender, ElevatorCall call)
        {
            LogEvent("Elevator Departed", ("From", call.SourceFloor), ("To", call.DestinationFloor));
        }

        private void ElevatorArriving(object sender, ElevatorCall e)
        {
            LogEvent("Elevator Arriving", ("At Floor", e.DestinationFloor));
        }

        private async void ElevatorArrived(object sender, ElevatorCall e)
        {
            LogEvent("Elevator Arrived", ("At Floor", e.DestinationFloor));
            IEnumerable<Passenger> leaving = this.Passengers.Where(p => p.Path.destination == e.DestinationFloor);
            foreach (Passenger passenger in leaving)
            {
                await this.RemovePassenger(passenger);
            }
        }

        public bool Subscribed { get; private set; }

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

        private async Task RemovePassenger(Passenger passenger)
        {
            LogEvent("Passenger Leaving Elevator");
            passenger.State = PassengerState.Transition;
            await Task.Delay(Passenger.TransitionSpeed);
            this._passengers.Remove(passenger);
            passenger.State = PassengerState.Out;
            PassengerExited?.Invoke(this, passenger);
        }

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

        ~Elevator()
        {
            // Decrement the global count of elevators
            if (Elevator._RegisteredElevators > 0)
                Elevator._RegisteredElevators -= 1;
        }
    }
}
