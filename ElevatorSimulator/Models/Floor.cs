using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using MoreLinq;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a floor of a building.
    /// </summary>
    public class Floor : ModelBase, ISubcriber<ElevatorMasterController>
    {
        /// <summary>
        /// Used to prevent threading issues
        /// </summary>
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        #region Backing fields
        private int _floorNum = 1;
        private ElevatorCallPanel _callPanel = new ElevatorCallPanel(1);

        private bool _elevatorAvailable = false;

        private readonly AsyncObservableCollection<Passenger> _waitingPassengers = new AsyncObservableCollection<Passenger>();
        // private readonly AsyncObservableCollection<Passenger> _waitingPassengers_Down = new AsyncObservableCollection<Passenger>();

        #endregion

        #region Properties

        /// <summary>
        /// The number of the floor
        /// </summary>
        public int FloorNumber
        {
            get => _floorNum;
            private set => SetProperty(ref _floorNum, value);

        }

        /// <summary>
        /// The panel outside of the <see cref="Elevator"/>, used to call the elevator to the floor.
        /// </summary>
        public ElevatorCallPanel CallPanel
        {
            get => _callPanel;
            set => SetProperty(ref _callPanel, value);
        }

        /// <summary>
        /// Represents whether the <see cref="Elevator"/> is at this floor at the given moment
        /// </summary>
        public bool ElevatorAvailable
        {
            get => _elevatorAvailable;
            private set => SetProperty(ref _elevatorAvailable, value);
        }

        /// <summary>
        /// The passengers waiting on the floor for the <see cref="Elevator"/>
        /// </summary>
        public IReadOnlyCollection<Passenger> WaitingPassengers => _waitingPassengers;

        #endregion

        /// <summary>
        /// Constructs a new <see cref="Floor"/> with the given floor number and call panel passed in
        /// </summary>
        public Floor(int floorNumber, ElevatorCallPanel callPanel)
        {
            this.FloorNumber = floorNumber;
            this.CallPanel = callPanel;
        }


        /// <summary>
        /// Constructs a new <see cref="Floor"/> with the given floor number
        /// </summary>
        public Floor(int floorNumber) : this(floorNumber, new ElevatorCallPanel(floorNumber))
        {

        }


        /// <summary>
        /// Parameterless constructor. Shouldn't be called directly. Only for use with the designer
        /// </summary>
        public Floor()
        {

        }

        /// <summary>
        /// Adds a passenger to the list of waiting passengers
        /// </summary>
        /// <param name="destination"></param>
        private void QueuePassenger(int destination)
        {
            var passenger = new Passenger(this.FloorNumber, destination);

            this._waitingPassengers.Add(passenger);
        }

        #region Subscription

        /// <summary>
        /// Represents whether or not this object has performed the necessary steps to subscribe to the source.
        /// </summary>
        public bool Subscribed { get; private set; }

        private int subscriptionCount = 0;

        /// <summary>
        /// Subscribes the <see cref="Floor"/> to the given master controller and elevator
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public async Task Subscribe(ElevatorMasterController controller)
        {
            if (this.Subscribed)
                return;

            foreach (FloorButton button in this.CallPanel)
            {
                button.OnPushed += (_, floorNum) =>
                {
                    this.QueuePassenger(floorNum);
                };
            }

            await this.CallPanel.Subscribe(controller);

            var elevator = controller.Elevator;

            if (elevator.CurrentFloor == this.FloorNumber)
            {
                this.ElevatorAvailable = true;
            }

            Logger.LogEvent("Subscribing floor", ("Subscription number", subscriptionCount));

            _waitingPassengers.CollectionChanged += onPassengerAdded;
            elevator.PassengerAdded += removePassengerFromQueue;
            elevator.Door.Opened += addPassengersToElevator;
            elevator.Door.CloseRequested += cancelIfStartsToClose;
            elevator.Door.Closing += cancelIfStartsToClose;
            elevator.Departed += onElevatorDeparted;

            elevator.Approaching += Elevator_Approaching;
            elevator.Arrived += onElevatorArrivedAtThisFloor;

            #region Local methods for event handlers, which do the real work

            // Respond to passengers trying to get on the elevator
            async void onPassengerAdded(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    // If the elevator is here, add the passengers and get going
                    if (this.ElevatorAvailable)
                    {
                        try
                        {
                            if (elevator.Door.DoorState != DoorState.Opened)
                            {

                                elevator.Door.RequestOpen();
                            }
                            else
                            {
                                IEnumerable<Passenger> departing = getPassengersToMove(elevator);
                                await _addPassengersToElevator(departing, elevator).ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            elevator.OnError(ex);
                        }
                    }
                }
            }

            async void addPassengersToElevator(object _, DoorStateChangeEventArgs args)
            {
                try
                {
                    if (elevator.CurrentFloor == this.FloorNumber)
                    {
                        await _addPassengersToElevator(elevator).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    elevator.OnError(ex);
                }
            }

            void Elevator_Approaching(object sender, ElevatorApproachingEventArgs e)
            {
                try
                {
                    if (e.IntermediateFloor == this.FloorNumber)
                    {
                        e.ShouldStop = getPassengersToMove(elevator).Any();
                    }
                }
                catch (Exception ex)
                {
                    elevator.OnError(ex);
                }
            }

            #endregion

            this.Subscribed = true;
        }

        /// <summary>
        /// Removes the passenger from queue of waiting passengers
        /// </summary>
        /// <param name="_">The .</param>
        /// <param name="passenger">The passenger.</param>
        void removePassengerFromQueue(object _, Passenger passenger)
        {
            try
            {
                if (this._waitingPassengers.TryRemove(passenger))
                {
                    ; // Successfully removed
                }
                else
                {
                    ; // Passenger was already removed
                }

            }
            catch (Exception ex)
            {
                if (ex is ArgumentOutOfRangeException)
                {
                    ; // Ignore
                }
            }
        }

        /// <summary>
        /// Alert the Floor that an elevator is available when one arrives on this floor
        /// </summary>
        /// <param name="_">The .</param>
        /// <param name="args">The <see cref="ElevatorApp.Models.ElevatorMovementEventArgs" /> instance containing the event data.</param>
        void onElevatorArrivedAtThisFloor(object _, ElevatorMovementEventArgs args)
        {
            if (args.DestinationFloor == this.FloorNumber)
            {
                this.ElevatorAvailable = true;
            }
        }

        /// <summary>
        /// Sets <see cref="ElevatorAvailable"/> to false when the elevator leaves the floor
        /// </summary>
        /// <param name="_">The sender</param>
        /// <param name="call">The <see cref="ElevatorApp.Models.ElevatorMovementEventArgs" /> instance containing the event data.</param>
        void onElevatorDeparted(object _, ElevatorMovementEventArgs call)
        {
            this.ElevatorAvailable = false;
        }

        /// <summary>
        /// Adds the passengers to elevator.
        /// </summary>
        /// <param name="elevator">The elevator.</param>
        /// <returns></returns>
        private async Task _addPassengersToElevator(Elevator elevator)
        {
            // TODO: Wait for all passengers to get out
            IEnumerable<Passenger> departing = getPassengersToMove(elevator);
            await _addPassengersToElevator(departing, elevator).ConfigureAwait(false);
        }

        private bool _adding = false;
        /// <summary>
        /// Sends a request to the <see cref="Door"/> to prevent it closing if starts to close while passengers are still trying to get on.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ElevatorApp.Models.DoorStateChangeEventArgs" /> instance containing the event data.</param>
        void cancelIfStartsToClose(object sender, DoorStateChangeEventArgs args)
        {
            args.CancelOperation = _adding;
        }

        /// <summary>
        /// Takes care of:
        /// <para>Opening the Elevator door</para>
        /// <para>Moving passengers from this <see cref="Floor"/> onto the <see cref="Elevator"/></para>
        /// <para>Keeping the door open until all <see cref="Passenger"/>s are added</para>
        /// </summary>
        /// <param name="departing"></param>
        /// <param name="elevator"></param>
        /// <returns></returns>
        private async Task _addPassengersToElevator(IEnumerable<Passenger> departing, Elevator elevator)
        {
            await _mutex.WaitAsync().ConfigureAwait(false);
            _adding = true;
            try
            {
                foreach (Passenger passenger in departing)
                {
                    await elevator.AddPassenger(passenger);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _adding = false;
                _mutex.Release();
            }

        }

        /// <summary>
        /// Gets the passengers that are going to be moving
        /// </summary>
        IEnumerable<Passenger> getPassengersToMove(Elevator elevator)
        {
            try
            {
                var direction = elevator.Direction;

                if (elevator.Direction == Direction.None)
                {
                    // Get the floor of the first passenger added to the waitlist. 
                    Passenger pass = this.WaitingPassengers.MinByOrDefault(p => p.PassengerNumber);

                    direction = pass?.Direction ?? Direction.None;
                }

                if (direction != Direction.None)
                    return this._getPassengersToMove(direction);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                //       mutex.Release();
            }

            return Enumerable.Empty<Passenger>();
        }

        /// <summary>
        /// Gets the passengers to move.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        private IEnumerable<Passenger> _getPassengersToMove(Direction direction)
        {
            IEnumerable<Passenger> waiting = this.WaitingPassengers.Where(p => p.State == PassengerState.Waiting);

            switch (direction)
            {
                // If the elevator is already going up, only add passengers who are going up
                case Direction.Up:

                    return waiting
                            .Where(p => p.Path.destination >= this.FloorNumber);

                // If the elevator is already going down, only add passengers who are going down
                case Direction.Down:
                    return waiting
                        .Where(p => p.Path.destination <= this.FloorNumber);

                default:
                    throw new InvalidOperationException("Direction cannot be None");
            }
        }


        #endregion

    }
}
