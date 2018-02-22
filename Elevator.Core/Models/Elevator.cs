using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Models
{

    public class Elevator : ModelBase, IElevator, ISubcriber<ElevatorMasterController>
    {
        #region Backing fields

        private int _speed, _capacity, _currentFloor;

        #endregion

        #region INotifyParent Properties

        public int Capacity
        {
            get => _capacity;
            set => SetValue(ref _capacity, value);
        }


        public int CurrentWeight => Passengers?.Count ?? 0;

        public ElevatorState State { get; set; }

        public int CurrentFloor
        {
            get => _currentFloor;
            private set => SetValue(ref _currentFloor, value);
        }

        #endregion


        public ObservableConcurrentQueue<int> Path { get; } = new ObservableConcurrentQueue<int>();
        public ICollection<IPassenger> Passengers { get; } = new ObservableCollection<IPassenger>();

        public ButtonPanel ButtonPanel { get; }

        public Door Door { get; } = new Door();

        public event EventHandler<int> OnArrival;
        public event EventHandler<int> OnDeparture;

        public void Subscribe(ElevatorMasterController controller)
        {
            this.ButtonPanel.Subscribe((controller, this));
        }

        public void Arrived()
        {
            if (this.Path.TryDequeue(out int destination))
            {
                Console.WriteLine("Arrived");
                this.OnArrival?.Invoke(this, destination);
            }
        }

        public void Dispatch(int floor)
        {
            this.Path.Enqueue(floor);
        }

        private void Move()
        {
            this.OnDeparture?.Invoke(this, this.CurrentFloor);

            // Logic
            Thread.Sleep(4000);
            this.Arrived();
        }

        private void Elevator_OnDeparture(object sender, int e)
        {
        }

        private void Elevator_OnArrival(object sender, int e)
        {
            this.Path.TryDequeue(out _);
        }


        public Elevator()
        {
            Logger.LogEvent($"Initializing Elevator {this.GetHashCode()}");
            this.ButtonPanel = new ButtonPanel();
            OnArrival += Elevator_OnArrival;
            OnDeparture += Elevator_OnDeparture;

        }

        public Elevator(int initialFloor) : this()
        {
            this.CurrentFloor = initialFloor;
        }
    }
}
