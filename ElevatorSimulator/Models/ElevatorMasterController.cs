using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Properties;
using ElevatorApp.Util;
using MoreLinq;

namespace ElevatorApp.Models
{
    public class ElevatorMasterController : ModelBase, IObserver<int>
    {
        #region Backing fields
        private int _floorHeight;

        private readonly AsyncObservableCollection<Elevator> _elevators = new AsyncObservableCollection<Elevator>(
            new[] {
                new Elevator(1)
            }
        );

        private readonly AsyncObservableCollection<Floor> _floors = new AsyncObservableCollection<Floor>(Enumerable.Range(1, 4).Reverse().Select(a => new Floor(a)));
        private readonly AsyncObservableCollection<int> _floorsRequested = new AsyncObservableCollection<int>();

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
        /// A Read-only collection of floor numbers that are currently waiting for an elevator
        /// </summary>
        public IReadOnlyCollection<int> FloorsRequested => _floorsRequested;

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

        public event EventHandler<int> OnElevatorRequested;
        #endregion

        private readonly SoundPlayer soundPlayer = new SoundPlayer { Stream = Resources.elevatorDing };

        #region Methods

        #region Private methods
        /// <summary>
        /// Runs when an elevator is has arrived
        /// </summary>
        /// <param name="elevator"></param>
        private async Task ElevatorArrived(Elevator elevator, Direction direction)
        {
            soundPlayer.Play();

            this._floorsRequested.TryRemove(elevator.CurrentFloor);

            //if (this._floorsRequested.TryPeek(out var val) && val == elevator.CurrentFloor)
            //{
            //    this._floorsRequested.TryDequeue(out _);
            //}

            //if (direction != Direction.None)
            //    foreach (var destination in this._floorsRequested)
            //    {
            //        await this.Dispatch(destination, elevator);
            //    }
        }

        /// <summary>
        /// Dispatches an <see cref="Elevator"/> to go to the given floor
        /// </summary>
        /// <param name="destination">The destination to be handled</param>
        /// <param name="elevator">The elevator to respond to the destination</param>
        private async Task Dispatch(int destination, Elevator elevator)
        {
            if (elevator != null)
                await elevator.Dispatch(destination).ConfigureAwait(false);
        }

        #endregion

        /// <summary>
        /// Dispatch an elevator to go to the given floor
        /// <para>This will add the <paramref name="destination"/> to <see cref="FloorsRequested"/> and be processed further by the <see cref="Elevator"/> that is chosen to respond</para>
        /// </summary>
        /// <param name="destination">The destination to be dispatched</param>
        public async Task Dispatch(int destination)
        {
            int distanceFromRequestedFloor(Elevator e) => Math.Abs(destination - e.CurrentFloor);

            // First check if any are not moving
            Elevator closest = Elevators
                .Where(e => e.Direction == ElevatorDirection.None)
                .MinByOrDefault(distanceFromRequestedFloor); // If any were found idle, the closest one to the requested floor is dispatched

            if (closest == null)
            {
                // If none were idle, find the one that's closest to it, going in the direction
                closest = this.Elevators
                    // TODO: reapply direction-based logic here
                    //.Where(e =>
                    //{
                    //    Direction direction = e.Direction;
                    //    return e.Direction == (ElevatorDirection) direction;
                    //})
                    .MinByOrDefault(distanceFromRequestedFloor);
            }

            if (closest != null)
                await Dispatch(destination, closest).ConfigureAwait(false);

        }

        /// <summary>
        /// Subscribes the elevators to this <see cref="ElevatorMasterController"/>
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            Logger.LogEvent($"Initializing {nameof(ElevatorMasterController)}");

            this.OnElevatorRequested += async (sender, destination) =>
             {
                 this._floorsRequested.AddDistinct(destination);
                 await this.Dispatch(destination).ConfigureAwait(false);
             };

            await Task.WhenAll(this.Elevators.Select(SubscribeElevator).Concat(this.Floors.Select(floor => floor.CallPanel.Subscribe(this))));

            Logger.LogEvent($"Initialized {nameof(ElevatorMasterController)}");
        }

        /// <summary>
        /// Runs the necessary functions to have the given <see cref="Elevator"/> respond appropriately to this <see cref="ElevatorMasterController"/>
        /// </summary>
        /// <param name="elevator">The elevator to be subscribed</param>
        private async Task SubscribeElevator(Elevator elevator)
        {
            await elevator.Subscribe(this);

            elevator.Arrived += async (a, b) =>
            {
                await this.ElevatorArrived(elevator, b.Direction);
            };

            Parallel.ForEach(elevator.ButtonPanel.FloorButtons, b =>
                b.OnPushed += delegate
                {
                    this.OnElevatorRequested?.Invoke(b, b.FloorNumber);
                });


            Parallel.ForEach(this.Floors, floor =>
            {
                foreach (FloorButton button in floor.CallPanel.FloorButtons)
                {
                    button.OnPushed += delegate
                      {
                          this.OnElevatorRequested?.Invoke(button, button.FloorNumber);
                      };
                }

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

        ///<inheritdoc/>
        /// <summary>
        /// Called when a floor has been requested
        /// </summary>
        void IObserver<int>.OnNext(int value)
        {
            Logger.LogEvent("Elevator requested", ("Floor", value));
            this._floorsRequested.AddDistinct(value);
        }

        ///<inheritdoc/>
        void IObserver<int>.OnError(Exception error)
        {
            // not implemented
        }

        ///<inheritdoc/>
        void IObserver<int>.OnCompleted()
        {
            // Not implemented
        }

    }

}
