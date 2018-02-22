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
    public class ElevatorMasterController : ModelBase, INotifyCollectionChanged, IElevatorMasterController
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private int _floorHeight;
        
        public ObservableCollection<Elevator> Elevators { get; set; } = new ObservableCollection<Elevator>(
            new[] {
                new Elevator(),
                new Elevator(2)
                {
                    Passengers=
                    {
                         new Passenger{ State= PassengerState.In, Weight=20}
                    }

                }
            }
        );

        public ObservableConcurrentQueue<ElevatorCall> FloorsRequested { get; } = new ObservableConcurrentQueue<ElevatorCall>();
        public ICollection<Floor> Floors { get; } = new ObservableCollection<Floor>(Enumerable.Range(0, 4).Select(a => new Floor(a)));


        public int FloorHeight
        {
            get => _floorHeight;
            set => SetValue(ref _floorHeight, value);
        }

        public ElevatorSettings ElevatorSettings { get; } = new ElevatorSettings();

        private void AdjustCollection<T>(ICollection<T> collection, int value, Func<int, T> generator) where T: ISubcriber<ElevatorMasterController>
        {
            if (collection.Count == value)
                return;

            int pseudoBackingField = 0;

            if (collection.Count < value)
            {
                for (int i = collection.Count; i < value; i++)
                {
                    T newItem = generator(i);
                    newItem.Subscribe(this);
                    collection.Add(newItem);
                    this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem));
                }
                SetValue(ref pseudoBackingField, collection.Count);

            }
            else
            {
                for (int i = collection.Count - 1; i >= value && i > 0; i--)
                {
                    T item = collection.LastOrDefault();
                    if (!Object.Equals(item, default))
                    {
                        collection.Remove(item);
                        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                    }
                }
                SetValue(ref pseudoBackingField, collection.Count);
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

        public event EventHandler<ElevatorCall> OnElevatorRequested;

        private void ElevatorArrived(Elevator elevator)
        {
            if (this.FloorsRequested.TryPeek(out ElevatorCall val) && val.DestinationFloor == elevator.CurrentFloor)
            {
                this.FloorsRequested.TryDequeue(out _);
            }
        }

        private void Dispatch(int floor, Elevator elevator)
        {
            elevator?.Dispatch(floor);
        }

        public void Dispatch(int floor, Direction direction)
        {
            int distanceFromRequestedFloor(Elevator e) => Abs(floor - e.CurrentFloor);

            // First check if any are not moving
            Elevator closest = Elevators
                .Where(e => e.State == ElevatorState.Idle)
                .MinBy(distanceFromRequestedFloor); // If any were found idle, the closest one to the requested floor is dispatched

            if (closest != null)
            {
                Dispatch(floor, closest);
            }
            else
            {
                // If none were idle, find the one that's closest to it, going in the direction
                closest = this.Elevators
                    .Where(e => e.State == (ElevatorState)direction)
                    .MinBy(distanceFromRequestedFloor);
            }

            Dispatch(floor, closest);
        }

        public void Dispatch(ElevatorCall call)
        {
            this.Dispatch(call.DestinationFloor, call.RequestDirection);
        }

        public ElevatorMasterController()
        {
            this.OnElevatorRequested = (sender, e) =>
            {
                this.FloorsRequested.Enqueue(e);
            };
        }

        public void ReportArrival(Elevator elevator)
        {
            ElevatorArrived(elevator);
        }

        public void Init()
        {
            foreach (Elevator item in this.Elevators)
            {
                item.Subscribe(this);
                item.OnArrival += (a, b) => this.ReportArrival(item);
            }
            

            Logger.LogEvent("Initialized");
        }

        private void ElevatorMasterController_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

    }
}
