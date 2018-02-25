using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;
using static System.Math;

namespace ElevatorApp.Core.Models
{
    public class ElevatorMasterController : ModelBase, IElevatorMasterController
    {
        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        private int _floorHeight;

        private readonly ObservableCollection<Elevator> _elevators = new ObservableCollection<Elevator>(
            new[] {
                new Elevator(),
                //new Elevator(2)
                //{
                //    Passengers=
                //    {
                //        new Passenger{ State= PassengerState.In, Weight=20}
                //    }

                //}
            }
        );

        private readonly ObservableCollection<Floor> _floors = new ObservableCollection<Floor>(Enumerable.Range(0, 4).Select(a => new Floor(a)));

        public ICollection<Elevator> Elevators => _elevators;
        public ICollection<Floor> Floors => _floors;

        public ObservableConcurrentQueue<ElevatorCall> FloorsRequested { get; } = new ObservableConcurrentQueue<ElevatorCall>();

        public int FloorHeight
        {
            get => _floorHeight;
            set => SetProperty(ref _floorHeight, value);
        }

        public ElevatorSettings ElevatorSettings { get; } = new ElevatorSettings();

        private void AdjustCollection<T>(ICollection<T> collection, int value, Func<int, T> generator) where T : ISubcriber<ElevatorMasterController>
        {
            if (collection.Count == value)
                return;

            if (value > collection.Count)
            {
                for (int i = collection.Count; i < value; i++)
                {
                    T newItem = generator(i);
                    newItem.Subscribe(this);
                    collection.Add(newItem);
                }
            }
            else
            {
                for (int i = collection.Count; i > value && i > 0; i--)
                {
                    T item = collection.LastOrDefault();
                    if (!Object.Equals(item, default))
                    {
                        collection.Remove(item);
                    }
                }
            }
        }

        public int FloorCount
        {
            get => Floors.Count;
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


        public void Dispatch(int floor, Direction direction)
        {
            this.Dispatch(new ElevatorCall(floor, direction));
        }
        
        private void Dispatch(ElevatorCall call, Elevator elevator)
        {
            elevator?.Dispatch(call);
        }

        public void Dispatch(ElevatorCall call)
        {
            int floor = call.DestinationFloor;
            Direction direction = call.RequestDirection;

            int distanceFromRequestedFloor(Elevator e) => Abs(floor - e.CurrentFloor);

            // First check if any are not moving
            Elevator closest = Elevators
                .Where(e => e.State == ElevatorState.Idle)
                .MinBy(distanceFromRequestedFloor); // If any were found idle, the closest one to the requested floor is dispatched

            if (closest != null)
            {
                Dispatch(call, closest);
            }
            else
            {
                // If none were idle, find the one that's closest to it, going in the direction
                closest = this.Elevators
                    .Where(e => e.State == (ElevatorState)direction)
                    .MinBy(distanceFromRequestedFloor);
            }

            Dispatch(call, closest);
        }

        public ElevatorMasterController()
        {
            this._elevators.CollectionChanged += (a, b) =>
            {
                base.OnPropertyChanged(this.ElevatorCount, nameof(ElevatorCount));
            };

            this._floors.CollectionChanged += (a, b) =>
            {
                base.OnPropertyChanged(this.FloorCount, nameof(FloorCount));
            };


            this.Init();
            this.OnElevatorRequested = (sender, e) =>
            {
                this.FloorsRequested.Enqueue(e);
            };
        }

        private void ReportArrival(Elevator elevator)
        {
            ElevatorArrived(elevator);
        }

        public void Init()
        {
            Logger.LogEvent("Initializing");
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
