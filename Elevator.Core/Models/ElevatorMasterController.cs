using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;
using static System.Math;

namespace ElevatorApp.Core.Models
{
    public class ElevatorMasterController : INotifyCollectionChanged, IElevatorMasterController
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        /// <summary>
        /// Singleton representation of elevator
        /// </summary>
        public static ElevatorMasterController Instance { get; } = new ElevatorMasterController();

        public ObservableCollection<Elevator> Elevators { get; set; } = new ObservableCollection<Elevator>(
            new[] {
                new Elevator(),
                new Elevator
                {
                    CurrentFloor = 2,
                    Passengers=
                    {
                         new Passenger{ State= PassengerState.In, Weight=20}
                    }

                }
            }
        );

        public ObservableConcurrentQueue<int> FloorsRequested { get; } = new ObservableConcurrentQueue<int>();
        public ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>(Enumerable.Range(0, 4).Select(a => new Floor(a)));

        public int FloorHeight { get; set; }

        public ElevatorSettings ElevatorSettings { get; set; } = new ElevatorSettings();

        private void AdjustCollection<T>(ICollection<T> collection, int value, Func<int, T> generator)
        {
            if (collection.Count == value)
                return;

            if (collection.Count < value)
            {
                for (int i = collection.Count; i < value; i++)
                {
                    collection.Add(generator(i));
                }
            }
            else
            {
                for (int i = collection.Count - 1; i >= value && i > 0; i--)
                {
                    T item = collection.LastOrDefault();
                    if (!Object.Equals(item, default))
                        collection.Remove(item);
                }
            }
        }

        public int FloorCount
        {
            get => Floors?.Count ?? 0;
            set => AdjustCollection(Floors, value, floorNum => new Floor(floorNum));
        }

        public int ElevatorCount
        {
            get => Elevators.Count;
            set => AdjustCollection(Elevators, value, _ => new Elevator());
        }

        public event EventHandler<int> OnElevatorRequested;

        public void QueueElevator(int floor)
        {
            OnElevatorRequested?.Invoke(this, floor);
        }

        public void QueueElevator(int floor, Direction direction)
        {
            OnElevatorRequested?.Invoke(this, floor);
        }

        private void ElevatorArrived(IElevator elevator)
        {
            if (this.FloorsRequested.TryPeek(out int val) && val == elevator.CurrentFloor)
            {
                this.FloorsRequested.TryDequeue(out _);
            }
        }

        private void Dispatch(int floor, IElevator elevator)
        {
            elevator.Dispatch(floor);
        }

        public void Dispatch(int floor, Direction direction)
        {
            int distanceFromRequestedFloor(IElevator e) => Abs(floor - e.CurrentFloor);

            // First check if any are not moving
            IElevator closest = Elevators
                .Where(e => e.State == ElevatorState.Idle)
                .MinBy(distanceFromRequestedFloor); // If any were found idle, the closest one to the requested floor is dispatched

            if (closest != null)
                Dispatch(floor, closest);

            // If none were idle, find the one that's closest to it, going in the direction
            closest = this.Elevators
                .Where(e => e.State == (ElevatorState)direction)
                .MinBy(distanceFromRequestedFloor);

            Dispatch(floor, closest);

        }

        public ElevatorMasterController()
        {
            this.OnElevatorRequested = (sender, e) =>
            {
                this.FloorsRequested.Enqueue(e);
            };
        }

        public void ReportArrival(IElevator elevator)
        {
            ElevatorArrived(elevator);
        }

        public void Init()
        {
            foreach (IElevator item in this.Elevators)
            {
                item.Subscribe(this);
            }
        }

        private void ElevatorMasterController_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
        
    }
}
