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
    public class ElevatorMasterController : ModelBase//, IElevatorMasterController
    {
        #region Backing fields
        private int _floorHeight;

        private readonly AsyncObservableCollection<Elevator> _elevators = new AsyncObservableCollection<Elevator>(
            new[] {
                new Elevator(1)
            }
        );

        private readonly AsyncObservableCollection<Floor> _floors = new AsyncObservableCollection<Floor>(Enumerable.Range(1, 4).Reverse().Select(a => new Floor(a)));
        private readonly AsyncObservableCollection<ElevatorCall> _floorsRequested = new AsyncObservableCollection<ElevatorCall>();
        #endregion

        #region Properties
        /// <summary>
        /// A Read-only collection of <see cref="Elevator"/> objects that will be managed by this <see cref="ElevatorMasterController"/>
        /// </summary>
        public IReadOnlyCollection<Elevator> Elevators => _elevators;

        /// <summary>
        /// <para>A Read-only collection of <see cref="Floor"/> objects that will be managed by this <see cref="ElevatorMasterController"/>.</para>
        /// <para>These represent the floors of the building.</para>
        /// </summary>
        public IReadOnlyCollection<Floor> Floors => _floors;

        /// <summary>
        /// A Read-only collection of <see cref="ElevatorCall"/> requests that are currently waiting to be fulfilled
        /// </summary>
        public IReadOnlyCollection<ElevatorCall> FloorsRequested => _floorsRequested;

        /// <summary>
        /// Represents the height of the floors in feet
        /// </summary>
        public int FloorHeight
        {
            get => _floorHeight;
            set => SetProperty(ref _floorHeight, value);
        }

        public ElevatorSettings ElevatorSettings { get; } = new ElevatorSettings();

        private void AdjustCollection<T>(ICollection<T> collection, int value, Func<int, T> generator, [CallerMemberName] string memberName = null) where T : ISubcriber<ElevatorMasterController>
        {
            if (collection.Count == value)
                return;

            if (value > collection.Count)
            {
                for (int i = collection.Count; i < value; i++)
                {
                    T newItem = generator(i);

                    if (newItem is Elevator e)
                    {
                        this.SubscribeElevator(e).GetAwaiter().GetResult();
                    }
                    else
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

        /// <summary>
        /// The number of floors currently being tracked.
        /// <para>
        /// <warning>Changing this number will change the number of elements in <see cref="Floors"/></warning>
        /// </para> 
        /// </summary>
        public int FloorCount
        {
            get => Floors.Count;
            set => AdjustCollection(_floors, value, floorNum => new Floor(floorNum));
        }

        /// <summary>
        /// The number of elevators currently being tracked.
        /// <para>
        /// <warning>Changing this number will change the number of elements in <see cref="Elevators"/></warning>
        /// </para> 
        /// </summary>
        public int ElevatorCount
        {
            get => Elevators.Count;
            set => AdjustCollection(_elevators, value, _ => new Elevator());
        }
        #endregion

        #region Events
        public event EventHandler<ElevatorCall> OnElevatorRequested;
        #endregion

        #region Methods

        #region Private methods
        /// <summary>
        /// Runs when an elevator is has arrived
        /// </summary>
        /// <param name="elevator"></param>
        private async Task ElevatorArrived(Elevator elevator)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.Stream = Properties.Resources.elevatorDing;
            player.Play();
            if (this._floorsRequested.TryPeek(out ElevatorCall val) && val.DestinationFloor == elevator.CurrentFloor)
            {
                this._floorsRequested.TryDequeue(out _);
            }

            foreach (ElevatorCall call in this._floorsRequested)
            {
                await this.Dispatch(call, elevator);
            }
        }

        /// <summary>
        /// Dispatches an <see cref="ElevatorCall"/> to be handled by the given <see cref="Elevator"/>
        /// </summary>
        /// <param name="call">The call to be handled</param>
        /// <param name="elevator">The elevator to respond to the call</param>
        private async Task Dispatch(ElevatorCall call, Elevator elevator)
        {
            if (elevator != null)
                await elevator.Dispatch(call).ConfigureAwait(false);
        }

        #endregion

        /// <summary>
        /// Dispatch a new <see cref="ElevatorCall"/>. 
        /// <para>This will add the <paramref name="call"/> to <see cref="FloorsRequested"/> and processed further by the <see cref="Elevator"/> that is chosen to respond</para>
        /// </summary>
        /// <param name="call">The call to be dispatched</param>
        public async Task Dispatch(ElevatorCall call)
        {
            if (!this._floorsRequested.Contains(call))
            {
                this._floorsRequested.Enqueue(call);
            }

            int floor = call.DestinationFloor;
            Direction direction = call.RequestDirection;

            int distanceFromRequestedFloor(Elevator e) => Math.Abs(floor - e.CurrentFloor);

            // First check if any are not moving
            Elevator closest = Elevators
                .Where(e => e.State == ElevatorState.Idle)
                .MinBy(distanceFromRequestedFloor); // If any were found idle, the closest one to the requested floor is dispatched

            if (closest == null)
            {
                // If none were idle, find the one that's closest to it, going in the direction
                closest = this.Elevators
                    .Where(e => e.State == (ElevatorState)direction)
                    .MinBy(distanceFromRequestedFloor);
            }

            if (closest != null)
                await Dispatch(call, closest).ConfigureAwait(false);

        }

        /// <summary>
        /// Subscribes the elevators to this <see cref="ElevatorMasterController"/>
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            Logger.LogEvent($"Initializing {nameof(ElevatorMasterController)}");

            await Task.WhenAll(this.Elevators.Select(SubscribeElevator));

            Logger.LogEvent($"Initialized {nameof(ElevatorMasterController)}");
        }

        /// <summary>
        /// Runs the necessary functions to have the given <see cref="Elevator"/> respond appropriately to this <see cref="ElevatorMasterController"/>
        /// </summary>
        /// <param name="elevator">The elevator to be subscribed</param>
        private async Task SubscribeElevator(Elevator elevator)
        {
            //this.OnElevatorRequested += async (sender, call) =>
            // {
            //     this._floorsRequested.Enqueue(call);
            //     await this.Dispatch(call).ConfigureAwait(false);
            // };

            await elevator.Subscribe(this);

            elevator.Arrived += async (a, b) => await this.ElevatorArrived(elevator);

            Parallel.ForEach(elevator.ButtonPanel.FloorButtons, b =>
                b.OnPushed += delegate
                {
                    this.OnElevatorRequested?.Invoke(b, new ElevatorCall(elevator.CurrentFloor, b.FloorNum));
                });


            Parallel.ForEach(this.Floors, floor =>
            {
                this.OnElevatorRequested?.Invoke(floor, new ElevatorCall(elevator.CurrentFloor, floor.FloorNumber));
            });

            await Task.WhenAll(this.Floors.Select(floor => floor.Subscribe((this, elevator))));
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="ElevatorMasterController"/>
        /// </summary>
        public ElevatorMasterController()
        {
            // Force execution immediately 
            this.Init().GetAwaiter().GetResult();
        }
    }
}
