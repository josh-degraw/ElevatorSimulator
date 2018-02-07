using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ElevatorApp.Core
{
    public class Elevator : NotifyPropertyChanged
    {

        #region Backing fields

        private int _speed = 0, _capacity = 0, _currentFloor = 1;
        private ElevatorState _state = ElevatorState.Idle;
        private IReadOnlyDictionary<int, FloorButton> _floorButtons = new Dictionary<int, FloorButton>();

        #endregion

        #region INotifyParent Properties
        public int Speed
        {
            get => _speed;
            set => SetValue(ref _speed, value);
        }

        public int Capacity
        {
            get => _capacity;
            set => SetValue(ref _capacity, value);
        }

        public ElevatorState State
        {
            get => _state;
            set => SetValue(ref _state, value);
        }


        public IReadOnlyDictionary<int, FloorButton> FloorButtons
        {
            get => _floorButtons;
            set => SetValue(ref _floorButtons, value);
        }

        public int CurrentFloor
        {
            get => _currentFloor;
            private set => SetValue(ref _currentFloor, value);
        }

        #endregion

        public ConcurrentQueue<int> Path { get; } = new ConcurrentQueue<int>();
        public ConcurrentBag<Passenger> Passengers { get; } = new ConcurrentBag<Passenger>();

        public DoorButton CloseDoorButton { get; } = new DoorButton(DoorButtonType.Close);
        public DoorButton OpenDoorButton { get; } = new DoorButton(DoorButtonType.Open);

        public Door Door { get; } = new Door();

        public event EventHandler<int> OnArrival = delegate { };
        public event EventHandler<int> OnDeparture = delegate { };

        public void Dispatch(int floor)
        {
            this.Path.Enqueue(floor);


            throw new NotImplementedException();
        }

        private void Move()
        {
            this.OnDeparture(this, this.CurrentFloor);

            // Logic


            if (this.Path.TryDequeue(out int destination))
            {
                this.OnArrival(this, destination);
            }
        }

        private void Elevator_OnDeparture(object sender, int e)
        {

        }

        private void Elevator_OnArrival(object sender, int e)
        {
        }

        public Elevator()
        {
            OnArrival += Elevator_OnArrival;
            OnDeparture += Elevator_OnDeparture;
        }
    }
}
