using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;

namespace ElevatorApp.Models
{

    public class Elevator : ModelBase, IElevator, ISubcriber<ElevatorMasterController>
    {
        private static int _RegisteredElevators = 0;

        #region Backing fields

        private int _speed, _capacity, _currentFloor=1;
        private ElevatorState _state;

        #endregion

        #region INotifyParent Properties

        public int Capacity
        {
            get => _capacity;
            set => SetProperty(ref _capacity, value);
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

        public ObservableConcurrentQueue<int> Path { get; } = new ObservableConcurrentQueue<int>();
        public ICollection<IPassenger> Passengers { get; } = new ObservableCollection<IPassenger>();

        public ButtonPanel ButtonPanel { get; }

        public Door Door { get; } = new Door();

        public event EventHandler<int> OnArrival;
        public event EventHandler<int> OnDeparture;


        public async void Arrived()
        {
            if (this.Path.TryDequeue(out int destination))
            {
                this.CurrentFloor = destination;
                Console.WriteLine("Arrived");
                // Simulate slowdown after arriving
                await Task.Delay(250);
                this.OnArrival?.Invoke(this, destination);

                this.MoveNext();
            }
        }

        private void MoveNext()
        {
            if (this.Path.TryPeek(out int next))
            {
                while (this.Door.DoorState != DoorState.Closed)
                {
                    // Wait until the door is closed to move
                    Thread.Sleep(2000);
                }

                this.State = this.CurrentFloor > next
                    ? ElevatorState.GoingUp
                    : ElevatorState.GoingDown;
            }
            else
            {
                this.State = ElevatorState.Idle;
            }

        }

        internal void Dispatch(ElevatorCall call)
        {
            this.Path.Enqueue(call.DestinationFloor);

            if (this.State == ElevatorState.Idle)
            {
                this.State = (ElevatorState)call.RequestDirection;
                this.Move();
            }
        }

        private readonly Duration FLOOR_MOVEMENT_SPEED = Duration.FromSeconds(5);

        private void Move()
        {
            this.OnDeparture?.Invoke(this, this.CurrentFloor);

            // Logic
            Thread.Sleep(FLOOR_MOVEMENT_SPEED.Milliseconds);
            this.Arrived();
        }

        private void Elevator_OnDeparture(object sender, int e)
        {
        }

        private void Elevator_OnArrival(object sender, int e)
        {
            this.Path.TryDequeue(out _);
        }

        public int ElevatorNumber { get; }

        public Elevator()
        {
            this.ElevatorNumber = ++_RegisteredElevators;

            Logger.LogEvent("Initializing Elevator", ("ElevatorNumber", this.ElevatorNumber));
            this.ButtonPanel = new ButtonPanel();
            OnArrival += Elevator_OnArrival;
            OnDeparture += Elevator_OnDeparture;
        }

        public Elevator(int initialFloor) : this()
        {
            this.CurrentFloor = initialFloor;
        }

        public bool Subscribed { get; private set; }

        public void Subscribe(ElevatorMasterController controller)
        {
            //if (Subscribed)
            //    return;
            Logger.LogEvent("Subcribing elevator to MasterController", ("Elevator Number", this.ElevatorNumber.ToString()));
            this.ButtonPanel.Subscribe((controller, this));
            this.Door.Subscribe(this);
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
