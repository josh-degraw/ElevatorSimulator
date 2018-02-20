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

    public class Elevator : IElevator
    {
        #region INotifyParent Properties
        public int Speed { get; set; }

        public int Capacity { get; set; }

        public int CurrentWeight => Passengers?.Count ?? 0;

        public ElevatorState State { get; set; }

        public int CurrentFloor
        {
            get;

#if !DEBUG
            private 
#endif

            set ;
        }

        #endregion
        

        public ObservableConcurrentQueue<int> Path { get; } = new ObservableConcurrentQueue<int>();
        public ObservableCollection<IPassenger> Passengers { get; } = new ObservableCollection<IPassenger>();

        public ButtonPanel ButtonPanel { get; set; } = new ButtonPanel();

        public Door Door { get; } = new Door();

        public event EventHandler<int> OnArrival;
        public event EventHandler<int> OnDeparture;

        public void Subscribe(ElevatorMasterController controller)
        {
            if (!controller.Elevators.Contains(this))
            {
                controller.Elevators.Add(this);
            }

            this.ButtonPanel.Subscribe(controller);
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
            OnArrival += Elevator_OnArrival;
            OnDeparture += Elevator_OnDeparture;

        }
    }
}
