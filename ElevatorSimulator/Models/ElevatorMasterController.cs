﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using MoreLinq;

namespace ElevatorApp.Models
{
    public class ElevatorMasterController : ModelBase, IElevatorMasterController
    {
        #region Backing fields
        private int _floorHeight;

        private readonly AsyncObservableCollection<Elevator> _elevators = new AsyncObservableCollection<Elevator>(
            new[] {
                new Elevator(1)
            }
        );

        private readonly AsyncObservableCollection<Floor> _floors = new AsyncObservableCollection<Floor>(Enumerable.Range(1, 4).Reverse().Select(a => new Floor(a)));
        #endregion
      
        #region Properties
        public ICollection<Elevator> Elevators => _elevators;
        public ICollection<Floor> Floors => _floors;

        public ObservableConcurrentQueue<ElevatorCall> FloorsRequested { get; } = new ObservableConcurrentQueue<ElevatorCall>();

        public int FloorHeight
        {
            get => _floorHeight;
            set => SetProperty(ref _floorHeight, value);
        }

        public ElevatorSettings ElevatorSettings { get; } = new ElevatorSettings();


        private void AdjustCollection<T>(ICollection<T> collection, int value, Func<int, T> generator,
            [CallerMemberName] string memberName = null) where T : ISubcriber<ElevatorMasterController>
        {
            if (collection.Count == value)
                return;

            if (value > collection.Count)
            {
                for (int i = collection.Count; i < value; i++)
                {
                    T newItem = generator(i);
                    newItem.Subscribe(this);
                    if (newItem is ISubcriber<(ElevatorMasterController, Elevator)> subscriber)
                    {
                        foreach (Elevator elevator in this.Elevators)
                        {
                            subscriber.Subscribe((this, elevator));
                        }
                    }
                    collection.Add(newItem);
                }
            }
            else
            {
                for (int i = collection.Count; i > value && i > 0; i--)
                {
                    T item = collection.LastOrDefault();

                    if (!Equals(item, default)) // If LastOrDefault returned default, 
                    {                           // the collection is empty, so don't do anything here
                        collection.Remove(item);
                    }
                }
            }
            this.OnPropertyChanged(collection.Count, memberName);
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
        #endregion

        #region Events
        public event EventHandler<ElevatorCall> OnElevatorRequested;
        #endregion

        #region Methods
        
        #region Private methods
        private Task ElevatorArrived(Elevator elevator)
        {
            if (this.FloorsRequested.TryPeek(out ElevatorCall val) && val.DestinationFloor == elevator.CurrentFloor)
            {
                this.FloorsRequested.TryDequeue(out _);
            }

            return Task.CompletedTask;
        }

        private async Task Dispatch(ElevatorCall call, Elevator elevator)
        {
            if (elevator != null)
                await Task.Run(() => elevator.Dispatch(call).ConfigureAwait(false));
        }

        private async Task ReportArrival(Elevator elevator)
        {
            await ElevatorArrived(elevator).ConfigureAwait(false);
        }

        #endregion
        
        public async Task Dispatch(int floor, Direction direction)
        {
            await this.Dispatch(new ElevatorCall(floor, direction)).ConfigureAwait(false);
        }

        public async Task Dispatch(ElevatorCall call)
        {
            int floor = call.DestinationFloor;
            Direction direction = call.RequestDirection;

            int distanceFromRequestedFloor(Elevator e) => Math.Abs(floor - e.CurrentFloor);

            // First check if any are not moving
            Elevator closest = Elevators
                .Where(e => e.State == ElevatorState.Idle)
                .MinBy(distanceFromRequestedFloor); // If any were found idle, the closest one to the requested floor is dispatched

            if (closest != null)
            {
                await Dispatch(call, closest).ConfigureAwait(false);
            }
            else
            {
                // If none were idle, find the one that's closest to it, going in the direction
                closest = this.Elevators
                    .Where(e => e.State == (ElevatorState)direction)
                    .MinBy(distanceFromRequestedFloor);

                if (closest != null)
                    await Dispatch(call, closest).ConfigureAwait(false);
            }

        }


        public async Task Init()
        {
            Logger.LogEvent($"Initializing {nameof(ElevatorMasterController)}");

            await Task.WhenAll(this.Elevators.Select(async elevator =>
            {
                await elevator.Subscribe(this);

                elevator.OnArrival += async (a, b) => await this.ReportArrival(elevator);

                await Task.WhenAll(this.Floors.Select(floor => floor.Subscribe((this, elevator))));

            }));

            Logger.LogEvent($"Initialized {nameof(ElevatorMasterController)}");
        }
        #endregion

        public ElevatorMasterController()
        {
            this.OnElevatorRequested = (sender, e) =>
            {
                this.FloorsRequested.Enqueue(e);
            };

            // Force execution immediately 
            this.Init().GetAwaiter().GetResult();
        }


    }
}
